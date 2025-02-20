using System;
using AutoMapper;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using Taxi.Client.ViewModels;
using Taxi.Client.Views;

namespace Taxi.Client;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Driver, DriverViewModel>();
            cfg.CreateMap<DriverViewModel, DriverSetDto>();
            cfg.CreateMap<PassengerGetDto, PassengerViewModel>();
            cfg.CreateMap<PassengerViewModel, PassengerSetDto>();
            cfg.CreateMap<VehicleGetDto, VehicleViewModel>();
            cfg.CreateMap<VehicleViewModel, VehicleSetDto>();
            cfg.CreateMap<VehicleClassification, VehicleClassificationViewModel>();
            cfg.CreateMap<VehicleClassificationViewModel, VehicleClassificationSetDto>();
            cfg.CreateMap<RideGetDto, RideViewModel>()
                .ForMember(destination => destination.RideTime,
                    options => { options.MapFrom(ride => ride.RideTime.ToString()); });

            cfg.CreateMap<RideViewModel, RideSetDto>()
                .ForMember(destination => destination.RideTime,
                    options =>
                    {
                        options.MapFrom(ride => Convert.ToUInt32(System.TimeSpan.Parse(ride.RideTime).TotalSeconds));
                    });

            cfg.CreateMap<Ride, RideViewModel>()
                .ForMember(destination => destination.RideTime,
                    options => { options.MapFrom(ride => ride.RideTime.ToString()); });

            cfg.CreateMap<CountPassengerRidesGetDto, CountPassengerRidesViewModel>();

            cfg.CreateMap<InfosAboutRidesGetDto, InfoAboutRidesViewModel>()
                .ForMember(destination => destination.AverageTime,
                    options => { options.MapFrom(infos => System.TimeSpan.Parse(infos.AverageTime)); })
                .ForMember(destination => destination.MaxTime,
                    options => { options.MapFrom(infos => System.TimeSpan.Parse(infos.MaxTime)); });
        });

        Locator.CurrentMutable.RegisterConstant(new ApiWrapper());
        Locator.CurrentMutable.RegisterConstant(config.CreateMapper(), typeof(IMapper));

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}