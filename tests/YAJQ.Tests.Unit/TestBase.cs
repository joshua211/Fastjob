﻿using System;
using YAJQ.Core;
using YAJQ.Core.JobProcessor;
using YAJQ.Core.Persistence;
using YAJQ.Core.Utils;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace YAJQ.Tests.Unit;

public abstract class TestBase
{
    protected IJobProcessorFactory fakeFactory;
    protected IJobPersistence fakePersistence;
    protected IServiceProvider fakeProvider;
    protected IJobRepository fakeRepository;
    protected ILogger logger;
    protected IModuleHelper moduleHelper;
    protected YAJQOptions options;
    protected TestService service;
    protected ITransientFaultHandler transientFaultHandler;

    public TestBase()
    {
        fakeProvider = Substitute.For<IServiceProvider>();
        fakeProvider.GetService(typeof(TestService)).Returns(service);
        fakePersistence = Substitute.For<IJobPersistence>();
        fakeRepository = Substitute.For<IJobRepository>();
        fakeRepository.AddJobAsync(default, default, default).ReturnsForAnyArgs(DefaultJobId);
        moduleHelper = new ModuleHelper();
        logger = Substitute.For<ILogger>();
        service = new TestService();
        options = new YAJQOptions();
        transientFaultHandler =
            new DefaultTransientFaultHandler(Substitute.For<ILogger<DefaultTransientFaultHandler>>(), options);
        fakeFactory = Substitute.For<IJobProcessorFactory>();
        fakeFactory.New().Returns(new DefaultJobProcessor(moduleHelper, fakeProvider,
            Substitute.For<ILogger<DefaultJobProcessor>>(), transientFaultHandler));
    }

    public string DefaultJobId => "XXXXXXXXXXXXXXXXX";

    public PersistedJob PersistedSyncJob(string? id = null) =>
        PersistedJob.Asap(Core.Persistence.JobId.With(id ?? DefaultJobId), TestService.SyncDescriptor());

    public PersistedJob PersistedAsyncJob(string? id = null) =>
        PersistedJob.Asap(Core.Persistence.JobId.With(id ?? DefaultJobId), TestService.AsyncDescriptor());

    public PersistedJob PersistedExcJob(string? id = null) =>
        PersistedJob.Asap(Core.Persistence.JobId.With(id ?? DefaultJobId), TestService.ExceptionDescriptor());
}