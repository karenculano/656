﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppBootstrapper.cs" company="HandBrake Project (http://handbrake.fr)">
//   This file is part of the HandBrake source code - It may be used under the terms of the GNU General Public License.
// </copyright>
// <summary>
//   The Castle Bootstrapper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HandBrake.Startup
{
    using System;
    using System.Collections.Generic;

    using Caliburn.Micro;

    using HandBrake.Services;
    using HandBrake.Services.Interfaces;
    using HandBrake.Services.Presets;
    using HandBrake.Services.Presets.Interfaces;
    using HandBrake.Services.Queue;
    using HandBrake.Services.Queue.Interfaces;
    using HandBrake.Services.Scan;
    using HandBrake.Services.Scan.Interfaces;
    using HandBrake.Utilities;
    using HandBrake.Utilities.Interfaces;
    using HandBrake.ViewModels;
    using HandBrake.ViewModels.Interfaces;

    using IEncode = HandBrake.Services.Encode.Interfaces.IEncode;
    using LibEncode = HandBrake.Services.Encode.LibEncode;

    /// <summary>
    /// The Castle Bootstrapper
    /// </summary>
    public class AppBootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppBootstrapper"/> class.
        /// </summary>
        public AppBootstrapper()
        {
            this.Initialize();
        }

        /// <summary>
        /// Configure Castle Windsor
        /// </summary>
        protected override void Configure()
        {
            this.container = new SimpleContainer();

            this.container.Singleton<IWindowManager, WindowManager>();
            this.container.Singleton<IEventAggregator, EventAggregator>();

            // Services
            this.container.Singleton<IUpdateService, UpdateService>();
            this.container.Singleton<IScan, LibScan>();
            this.container.Singleton<IEncode, LibEncode>();
            this.container.Singleton<IPrePostActionService, PrePostActionService>();
            this.container.Singleton<IUserSettingService, UserSettingService>();
            this.container.Singleton<IPresetService, PresetService>();
            this.container.Singleton<IQueueProcessor, QueueProcessor>();

            // Utilities
            this.container.Singleton<ISystemInfo, SystemInfo>();
            this.container.Singleton<IProcessIdentificationService, ProcessIdentificationService>();
            this.container.Singleton<IDialogService, DialogService>();
            this.container.Singleton<LauncherServiceBase, LauncherService>();
            this.container.Singleton<ISoundService, SoundService>();
            this.container.Singleton<ITaskBarService, TaskBarService>();
            this.container.Singleton<ICopyService, CopyService>();
            this.container.Singleton<IViewManager, ViewManager>();
            this.container.Singleton<INotificationService, NotificationService>();

            // Services and Shell Components
            this.container.Singleton<IErrorService, ErrorService>();
            this.container.Singleton<IErrorViewModel, ErrorViewModel>();
            this.container.Singleton<IMainViewModel, MainViewModel>();
            this.container.Singleton<IQueueViewModel, QueueViewModel>();
            this.container.PerRequest<IAddPresetViewModel, AddPresetViewModel>();
            this.container.PerRequest<IManagePresetViewModel, ManagePresetViewModel>();
            this.container.Singleton<ILogViewModel, LogViewModel>();
            this.container.Singleton<IAboutViewModel, AboutViewModel>();
            this.container.Singleton<IOptionsViewModel, OptionsViewModel>();
            this.container.Singleton<ITitleSpecificViewModel, TitleSpecificViewModel>();
            this.container.Singleton<IQueueSelectionViewModel, QueueSelectionViewModel>();
            this.container.Singleton<ICountdownAlertViewModel, CountdownAlertViewModel>();
            this.container.Singleton<IMiniViewModel, MiniViewModel>();
            this.container.Singleton<IStaticPreviewViewModel, StaticPreviewViewModel>();

            // Tab Components
            this.container.Singleton<IAudioViewModel, AudioViewModel>();
            this.container.Singleton<IX264ViewModel, X264ViewModel>();
            this.container.Singleton<IPictureSettingsViewModel, PictureSettingsViewModel>();
            this.container.Singleton<IChaptersViewModel, ChaptersViewModel>();
            this.container.Singleton<ISubtitlesViewModel, SubtitlesViewModel>();
            this.container.Singleton<IFiltersViewModel, FiltersViewModel>();
            this.container.Singleton<IVideoViewModel, VideoViewModel>();
            this.container.Singleton<IMetaDataViewModel, MetaDataViewModel>();
            this.container.Singleton<ISummaryViewModel, SummaryViewModel>();

            // Shell
            this.container.Singleton<IShellViewModel, ShellViewModel>();

            base.Configure();
        }

        /// <summary>
        /// The on startup.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<IShellViewModel>();
        }

        /// <summary>
        /// Get an Instance of a service
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The Service Requested
        /// </returns>
        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);
            if (instance != null)
            {
                return instance;
            }

            throw new InvalidOperationException("Could not locate any instances.");
        }

        /// <summary>
        /// Get all instances of a service
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <returns>
        /// A collection of instances of the requested service type.
        /// </returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        /// <summary>
        /// Build Up
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        protected override void BuildUp(object instance)
        {
            this.container.BuildUp(instance);
        }
    }
}