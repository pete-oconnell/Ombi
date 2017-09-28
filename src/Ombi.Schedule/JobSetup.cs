﻿using Hangfire;
using Ombi.Schedule.Jobs;
using Ombi.Schedule.Jobs.Emby;
using Ombi.Schedule.Jobs.Plex;
using Ombi.Schedule.Jobs.Radarr;
using Ombi.Schedule.Ombi;

namespace Ombi.Schedule
{
    public class JobSetup : IJobSetup
    {
        public JobSetup(IPlexContentCacher plexContentCacher, IRadarrCacher radarrCacher,
            IOmbiAutomaticUpdater updater, IEmbyContentCacher embyCacher, IPlexUserImporter userImporter,
            IEmbyUserImporter embyUserImporter)
        {
            PlexContentCacher = plexContentCacher;
            RadarrCacher = radarrCacher;
            Updater = updater;
            EmbyContentCacher = embyCacher;
            PlexUserImporter = userImporter;
            EmbyUserImporter = embyUserImporter;
        }

        private IPlexContentCacher PlexContentCacher { get; }
        private IRadarrCacher RadarrCacher { get; }
        private IOmbiAutomaticUpdater Updater { get; }
        private IPlexUserImporter PlexUserImporter { get; }
        private IEmbyContentCacher EmbyContentCacher { get; }
        private IEmbyUserImporter EmbyUserImporter { get; }

        public void Setup()
        {
            RecurringJob.AddOrUpdate(() => PlexContentCacher.CacheContent(), Cron.Hourly(20));
            RecurringJob.AddOrUpdate(() => EmbyContentCacher.Start(), Cron.Hourly(5));
            RecurringJob.AddOrUpdate(() => RadarrCacher.CacheContent(), Cron.Hourly(10));
            RecurringJob.AddOrUpdate(() => PlexUserImporter.Start(), Cron.Daily(1));
            RecurringJob.AddOrUpdate(() => EmbyUserImporter.Start(), Cron.Daily);
            RecurringJob.AddOrUpdate(() => Updater.Update(null), Cron.Daily(3));

            //BackgroundJob.Enqueue(() => PlexUserImporter.Start());
        }
    }
}
