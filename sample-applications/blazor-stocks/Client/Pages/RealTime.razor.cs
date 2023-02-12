using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using BlazorFinancePortfolio.Models;
using BlazorFinancePortfolio.Services;
using BlazorPro.BlazorSize;
using Telerik.Blazor.Components;

namespace BlazorFinancePortfolio.Client.Pages;

public partial class RealTime : IDisposable
{
    [Inject] RealTimeDataService RealTimeDataService { get; set; }
    [Inject] ResizeListener listener { get; set; }
    [CascadingParameter] public Currency SelectedCurrency { get; set; }
    bool ShowAllColumns { get; set; }
    int LoadDataInterval { get; set; } = 1500;
    List<RealTimeData> GridData { get; set; }
    CancellationTokenSource CancelToken;

    int LastViewPortWidth { get; set; }

    private void OnRowRenderHandler(GridRowRenderEventArgs args)
    {
        var model = args.Item as RealTimeData;
        //args.Class = model.Change > 0 ? "animate__animated animate__flash" : "";
    }


    protected override async Task OnInitializedAsync()
    {
        await ToggleColumns(null);
        CancelToken = new CancellationTokenSource();
        GridData = await RealTimeDataService.GetInitialData(SelectedCurrency.Symbol);
        await IntervalDataUpdate();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            listener.OnResized += WindowResizeHandler;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    async Task IntervalDataUpdate()
    {
        while (CancelToken.Token != null)
        {
            await Task.Delay(LoadDataInterval, CancelToken.Token);
            await RefreshData();
            StateHasChanged();
        }
    }

    void StopTimer()
    {
        CancelToken?.Cancel();
    }

    async void WindowResizeHandler(object _, BrowserWindowSize window)
    {
        if (LastViewPortWidth != window.Width)
        {
            LastViewPortWidth = window.Width;
            await ToggleColumns(window.Width);
        }
    }

    protected async Task ToggleColumns(int? windowWidth)
    {
        if (windowWidth == null)
        {
            BrowserWindowSize currSize = await listener.GetBrowserWindowSize();
            windowWidth = currSize.Width;
            LastViewPortWidth = windowWidth.Value;
        }

        if (windowWidth < 992)
        {
            ShowAllColumns = false;
        }
        else
        {
            ShowAllColumns = true;
        }

        StateHasChanged();
    }

    public void Dispose()
    {
        listener.OnResized -= WindowResizeHandler;
        StopTimer();
    }

    async Task RefreshData()
    {
        foreach (RealTimeData item in GridData)
        {
            decimal change = RealTimeDataService.GetRandomChange();
            item.Change = change;
            item.Price += change;
        }
    }

    string GetPriceChangeClass(decimal change)
    {
        if (change > 0) return "price-up";
        if (change < 0) return "price-down";
        return "";
    }
}