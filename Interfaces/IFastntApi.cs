using EPCISEvent.Interfaces.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPCISEvent.Interfaces
{
    public interface IFastntApi
    {
        #region Authentication
        [Post("/auth/login")]
        Task<Result<AuthResponse>> Login([Body] DTOs.LoginRequest loginRequest);

        [Post("/auth/refresh")]
        Task<Result<AuthResponse>> RefreshToken([Body] RefreshTokenRequest refreshRequest);
        #endregion

        #region Capture Endpoints
        [Post("/Capture")]
        [Headers("Content-Type: application/xml", "Accept: application/json")]
        Task<Result<CaptureResponse>> CaptureXmlEvent([Body] string xmlEvent);

        [Post("/Capture")]
        [Headers("Content-Type: application/json", "Accept: application/json")]
        Task CaptureJsonEvent([Body] object jsonEvent);

        [Post("/events")]
        [Headers("Content-Type: application/json")]
        Task<Result<EventResponse>> CaptureSingleEvent([Body] object eventData);
        #endregion

        #region Query Endpoints
        [Get("/v2_0/queries")]
        Task<Result<List<QueryDefinitionDto>>> ListQueries();

        [Get("/v2_0/queries/{queryName}")]
        Task<Result<QueryDefinitionDto>> GetQueryDetails(string queryName);

        [Post("/v2_0/queries")]
        Task<Result<QueryDefinitionDto>> CreateQuery([Body] QueryDefinitionDto queryDefinition);

        [Delete("/v2_0/queries/{queryName}")]
        Task<Result<bool>> DeleteQuery(string queryName);

        [Get("/v2_0/queries/{queryName}/events")]
        Task<Result<QueryResultsDto>> GetQueryEvents(string queryName);

        [Get("/events")]
        Task<Result<QueryResultsDto>> GetAllEvents();

        [Get("/eventTypes/{eventType}/events")]
        Task<Result<QueryResultsDto>> GetEventsByType(string eventType);
        #endregion

        #region Subscription Endpoints
        [Post("/queries/{queryName}/subscriptions")]
        Task<Result<SubscriptionDto>> CreateSubscription(string queryName, [Body] SubscriptionRequest subscriptionRequest);

        [Get("/queries/{queryName}/subscriptions")]
        Task<Result<List<SubscriptionDto>>> GetSubscriptions(string queryName);

        [Delete("/queries/{queryName}/subscriptions/{subscriptionId}")]
        Task<Result<bool>> DeleteSubscription(string queryName, string subscriptionId);
        #endregion

        #region Health & System
        [Get("/health")]
        Task<Result<string>> GetHealthStatus();

        [Get("/system/info")]
        Task<Result<SystemInfo>> GetSystemInfo();
        #endregion

        #region MasterData Vocabulary
        [Get("/vocabulary")]
        Task<Result<List<VocabularyDto>>> GetVocabularies();

        [Post("/vocabulary")]
        Task<Result<VocabularyDto>> CreateVocabulary([Body] VocabularyDto vocabulary);

        [Get("/vocabulary/{vocabularyType}")]
        Task<Result<VocabularyDto>> GetVocabularyByType(string vocabularyType);

        [Put("/vocabulary/{vocabularyType}")]
        Task<Result<VocabularyDto>> UpdateVocabulary(string vocabularyType, [Body] VocabularyDto vocabulary);

        [Delete("/vocabulary/{vocabularyType}")]
        Task<Result<bool>> DeleteVocabulary(string vocabularyType);
        #endregion
    }
}
