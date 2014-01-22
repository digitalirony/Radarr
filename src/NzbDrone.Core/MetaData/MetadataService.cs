﻿using NLog;
using NzbDrone.Core.MediaFiles.Events;
using NzbDrone.Core.Messaging.Events;
using NzbDrone.Core.Tv.Events;

namespace NzbDrone.Core.Metadata
{
    public class NotificationService
        : IHandle<SeriesUpdatedEvent>,
          IHandle<EpisodeImportedEvent>,
          IHandle<SeriesRenamedEvent>
    {
        private readonly IMetadataFactory _metadataFactory;
        private readonly IMetadataRepository _metadataRepository;
        private readonly Logger _logger;

        public NotificationService(IMetadataFactory metadataFactory, IMetadataRepository metadataRepository, Logger logger)
        {
            _metadataFactory = metadataFactory;
            _metadataRepository = metadataRepository;
            _logger = logger;
        }

        public void Handle(SeriesUpdatedEvent message)
        {
            foreach (var consumer in _metadataFactory.Enabled())
            {
                consumer.OnSeriesUpdated(message.Series);
            }
        }

        public void Handle(EpisodeImportedEvent message)
        {
            foreach (var consumer in _metadataFactory.Enabled())
            {
                consumer.OnEpisodeImport(message.EpisodeInfo.Series, message.ImportedEpisode, message.NewDownload);
            }
        }

        public void Handle(SeriesRenamedEvent message)
        {
            foreach (var consumer in _metadataFactory.Enabled())
            {
                consumer.AfterRename(message.Series);
            }
        }
    }
}
