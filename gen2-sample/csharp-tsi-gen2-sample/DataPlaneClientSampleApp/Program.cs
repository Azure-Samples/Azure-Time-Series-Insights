using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.TimeSeriesInsights;
using Microsoft.Azure.TimeSeriesInsights.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Newtonsoft.Json;
using DateTimeRange = Microsoft.Azure.TimeSeriesInsights.Models.DateTimeRange;

namespace DataPlaneSampleApp
{
    public sealed class Program
    {
        private const string ResourceUri = "120d688d-1518-4cf7-bd38-182f158850b6";
        private const string PowerShellAadClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        private const string AzureActiveDirectoryLoginUrl = "https://login.windows.net";
        private const string MicrosoftTenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47";

        // This can be found under time series environment resource on Azure portal under "Data Access FQDN".
        // This is the "Contoso Wind Farm" environment that can be found at - "https://insights.timeseries.azure.com/preview/samples"
        private const string EnvironmentFqdn = "10000000-0000-0000-0000-100000000109.env.timeseries.azure.com";

        // Select a timeSeriesId from the environment.
        private static readonly object[] TimeSeriesId = { "2da181d7-8346-4cf2-bd94-a17742237429" };

        // Select a search span for the query.
        private static readonly DateTimeRange SearchSpan = new DateTimeRange(new DateTime(2017, 12, 31).ToUniversalTime(), new DateTime(2018, 01, 01).ToUniversalTime());
        private static readonly Uri RedirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob");

        private static TimeSeriesInsightsClient _client;

        static void Main(string[] args)
        {
            _client = GetTimeSeriesInsightsClientAsync().Result;

            while (true)
            {
                RunOperationsLoopAsync().Wait();
            }
        }

        private class Operation
        {
            public string Description { get; }
            public Func<Task> Action { get; }

            public Operation(string description, Func<Task> action)
            {
                Description = description;
                Action = action;
            }
        }

        private static async Task RunOperationsLoopAsync()
        {
            Console.WriteLine();
            Console.WriteLine("Choose one of the operations:");
            Console.WriteLine("Environment: {0}", EnvironmentFqdn);
            Console.WriteLine("TimeSeriesId: {0}", TimeSeriesId);
            Console.WriteLine("SearchSpan: From: {0}, To: {1}", SearchSpan.FromProperty, SearchSpan.To);

            foreach (var op in Operations)
            {
                Console.WriteLine($"{op.Key}\t{op.Value.Description}");
            }

            try
            {
                Console.Write("Select an operation to run: ");
                int selection = int.Parse(Console.ReadLine());

                await Operations[selection].Action();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static readonly Dictionary<int, Operation> Operations = new Dictionary<int, Operation>
        {
            [0] = new Operation("Availability", GetAvailabilityAsync),
            [1] = new Operation("EventSchema", GetEventSchemaAsync),
            [2] = new Operation("Query/AggregateSeries", RunAggregateSeriesAsync),
            [3] = new Operation("Query/GetSeries", RunGetSeriesAsync),
            [4] = new Operation("Query/GetEvents", RunGetEventsAsync),
            [5] = new Operation("Query/GetEventsWithProjectedProperties", RunGetEventsWithProjectedPropertiesAsync),
            [6] = new Operation("GetInstances", RunGetInstancesAsync),
            [7] = new Operation("InstancesBatch", RunInstancesBatchAsync),
            [8] = new Operation("GetTypes", RunGetTypesAsync),
            [9] = new Operation("TypesBatch", RunTypesBatchAsync),
            [10] = new Operation("GetHierarchies", RunGetHierarchiesAsync),
            [11] = new Operation("HierarchiesBatch", RunHierarchiesBatchAsync),
            [12] = new Operation("Search", RunSearchBatchAsync),
        };

        private static async Task RunSearchBatchAsync()
        {
            SearchInstancesRequest searchRequest = new SearchInstancesRequest(
                "GearboxSensor",
                new List<string>() { "Contoso WindFarm Hierarchy" },
                new SearchInstancesParameters(true, new InstancesSortParameter(InstancesSortBy.Rank), true, 10),
                new SearchInstancesHierarchiesParameters(
                    new HierarchiesExpandParameter(HierarchiesExpandKind.UntilChildren),
                    new HierarchiesSortParameter(HierarchiesSortBy.CumulativeInstanceCount),
                    10));
            SearchInstancesResponsePage searchResponse = await _client.TimeSeriesInstances.SearchAsync(searchRequest);
            PrintResponse(searchResponse);
        }

        private static void PrintResponse(SearchInstancesResponsePage searchResponse)
        {
            Console.WriteLine("Search Results: {0}", JsonConvert.SerializeObject(searchResponse, Formatting.Indented));
        }

        private static async Task RunHierarchiesBatchAsync()
        {
            HierarchiesBatchResponse hierarchies =
                await _client.TimeSeriesHierarchies.ExecuteBatchAsync(new HierarchiesBatchRequest(get: new HierarchiesRequestBatchGetDelete(names: new List<string>() { "Contoso WindFarm Hierarchy" })));

            PrintResponse(hierarchies.Get.First().Hierarchy);
        }

        private static async Task RunGetHierarchiesAsync()
        {
            string continuationToken;
            do
            {
                GetHierarchiesPage hierarchies = await _client.TimeSeriesHierarchies.ListAsync();

                PrintResponse(hierarchies.Hierarchies);
                continuationToken = hierarchies.ContinuationToken;
            }
            while (continuationToken != null);
        }

        private static void PrintResponse(IList<TimeSeriesHierarchy> hierarchies)
        {
            Console.WriteLine("Hierarchies");
            foreach (TimeSeriesHierarchy hierarchy in hierarchies)
            {
                PrintResponse(hierarchy);
            }
        }

        private static void PrintResponse(TimeSeriesHierarchy hierarchy)
        {
            Console.WriteLine("Hierarchy:");
            Console.WriteLine("  ID: {0}", hierarchy.Id);
            Console.WriteLine("  Name: {0}", hierarchy.Name);
            Console.WriteLine("  Source: {0}", string.Join(" > ", hierarchy.Source.InstanceFieldNames));
        }

        private static async Task RunTypesBatchAsync()
        {
            TypesBatchResponse types = await _client.TimeSeriesTypes.ExecuteBatchAsync(
                new TypesBatchRequest(
                    get: new TypesRequestBatchGetOrDelete(typeIds: new List<string>() { "1be09af9-f089-4d6b-9f0b-48018b5f7393" })));

            PrintResponse(types.Get.First().TimeSeriesType);
        }

        private static async Task RunGetTypesAsync()
        {
            string continuationToken;
            do
            {
                GetTypesPage types = await _client.TimeSeriesTypes.ListAsync();

                PrintResponse(types.Types);
                continuationToken = types.ContinuationToken;
            }
            while (continuationToken != null);
        }

        private static void PrintResponse(IList<TimeSeriesType> types)
        {
            Console.WriteLine("Types: ");
            foreach (TimeSeriesType type in types)
            {
                PrintResponse(type);
            }
        }

        private static void PrintResponse(TimeSeriesType type)
        {
            Console.WriteLine("Type ID: {0}", type.Id);
            Console.WriteLine("  Name: {0}", type.Name);
            Console.WriteLine("  Description: {0}", type.Description);
            foreach (KeyValuePair<string, Variable> variable in type.Variables)
            {
                Console.WriteLine("  Variable: {0}", variable.Key);
                Console.WriteLine("    Filter: {0}", variable.Value.Filter?.TsxProperty);
                var numericVariable = variable.Value as NumericVariable;
                if (numericVariable != null)
                {
                    Console.WriteLine("    Value: {0}", numericVariable.Value?.TsxProperty);
                    Console.WriteLine("    Aggregation: {0}", numericVariable.Aggregation?.TsxProperty);
                }

                var aggregateVariable = variable.Value as AggregateVariable;
                if (aggregateVariable != null)
                {
                    Console.WriteLine("    Aggregation: {0}", aggregateVariable.Aggregation?.TsxProperty);
                }
            }
        }

        private static async Task RunInstancesBatchAsync()
        {
            InstancesBatchResponse instances = await _client.TimeSeriesInstances.ExecuteBatchAsync(
                new InstancesBatchRequest(get: new InstancesRequestBatchGetOrDelete(new IList<object>[] { TimeSeriesId })));

            PrintResponse(instances.Get.First().Instance);
        }

        private static async Task RunGetInstancesAsync()
        {
            string continuationToken;

            // Limit the total instances received.
            int limit = 1000;
            int totalInstanceCount = 0;
            TimeSeriesInstance firstInstance = null;
            do
            {
                GetInstancesPage instancesPage = await _client.TimeSeriesInstances.ListAsync();

                if (instancesPage.Instances != null)
                {
                    totalInstanceCount += instancesPage.Instances.Count;

                    Console.WriteLine("Received instances : {0}", totalInstanceCount);

                    if (firstInstance == null)
                    {
                        firstInstance = instancesPage.Instances.FirstOrDefault();
                    }
                }

                continuationToken = instancesPage.ContinuationToken;
            }
            while (continuationToken != null && totalInstanceCount < limit);

            Console.WriteLine("First Instance:");
            PrintResponse(firstInstance);
        }

        private static void PrintResponse(TimeSeriesInstance timeSeriesInstance)
        {
            Console.WriteLine("Time Series Instance");
            Console.WriteLine("  Time Series ID: {0}", string.Join(", ", timeSeriesInstance.TimeSeriesId));
            Console.WriteLine("  Description: {0}", timeSeriesInstance.Description);
            Console.WriteLine("  Type ID: {0}", timeSeriesInstance.TypeId);
            Console.WriteLine("  Hierarchy IDs: \n    {0}\n", timeSeriesInstance.HierarchyIds != null ? string.Join("\n    ", timeSeriesInstance.HierarchyIds) : "null");
            Console.WriteLine("  Instance Fields: \n    {0}\n", timeSeriesInstance.InstanceFields != null ? string.Join("\n    ", timeSeriesInstance.InstanceFields.Select(i => $"{i.Key}: {i.Value}")) : "null");
        }

        private static async Task GetAvailabilityAsync()
        {
            AvailabilityResponse availability = await _client.Query.GetAvailabilityAsync();
            PrintResponse(availability.Availability);
        }

        private static async Task GetEventSchemaAsync()
        {
            EventSchema eventSchema = await _client.Query.GetEventSchemaAsync(new GetEventSchemaRequest(SearchSpan));
            PrintResponse(eventSchema);
        }

        private static async Task RunAggregateSeriesAsync()
        {
            string continuationToken;
            do
            {
                QueryResultPage queryResponse = await _client.Query.ExecuteAsync(
                    new QueryRequest(
                        aggregateSeries: new AggregateSeries(
                            timeSeriesId: TimeSeriesId,
                            searchSpan: SearchSpan,
                            filter: null,
                            interval: TimeSpan.FromMinutes(5),
                            projectedVariables: new[] { "Min_Numeric", "Max_Numeric", "Sum_Numeric", "Avg_Numeric", "First_Numeric", "Last_Numeric", "SampleInterpolated_Numeric_Step_NoBoundary", "SampleInterpolated_Numeric_Step", "SampleInterpolated_Numeric_Linear_NoBoundary", "SampleInterpolated_Numeric_Linear", "Categorical_NonInterpolated", "Categorical_Interpolated", "Count_Aggregate" },
                            inlineVariables: new Dictionary<string, Variable>()
                            {
                                ["Min_Numeric"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("min($value)")),
                                ["Max_Numeric"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("max($value)")),
                                ["Sum_Numeric"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("sum($value)")),
                                ["Avg_Numeric"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("avg($value)")),
                                ["First_Numeric"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("first($value)")),
                                ["Last_Numeric"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("last($value)")),
                                ["SampleInterpolated_Numeric_Step_NoBoundary"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("left($value)"),
                                    interpolation: new Interpolation(kind: "Step")),
                                ["SampleInterpolated_Numeric_Step"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("left($value)"),
                                    interpolation: new Interpolation(kind: "Step", boundary: new InterpolationBoundary(TimeSpan.FromMinutes(1)))),
                                ["SampleInterpolated_Numeric_Linear_NoBoundary"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("left($value)"),
                                    interpolation: new Interpolation(kind: "Linear")),
                                ["SampleInterpolated_Numeric_Linear"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("left($value)"),
                                    interpolation: new Interpolation(kind: "Linear", boundary: new InterpolationBoundary(TimeSpan.FromMinutes(1)))),
                                ["Categorical_NonInterpolated"] = new CategoricalVariable(
                                    value: new Tsx("tolong($event.data)"),
                                    categories: new List<TimeSeriesAggregateCategory>()
                                    {
                                        new TimeSeriesAggregateCategory(label: "Good", values: new List<object>(){39}),
                                        new TimeSeriesAggregateCategory(label: "Bad", values: new List<object>(){40}),
                                        new TimeSeriesAggregateCategory(label: "OK", values: new List<object>(){41}),
                                        new TimeSeriesAggregateCategory(label: "Reject", values: new List<object>(){42})
                                    },
                                    defaultCategory: new TimeSeriesDefaultCategory("Others")),
                                ["Categorical_Interpolated"] = new CategoricalVariable(
                                    value: new Tsx("tolong($event.data)"),
                                    categories: new List<TimeSeriesAggregateCategory>()
                                    {
                                        new TimeSeriesAggregateCategory(label: "Good", values: new List<object>(){39}),
                                        new TimeSeriesAggregateCategory(label: "Bad", values: new List<object>(){40}),
                                        new TimeSeriesAggregateCategory(label: "OK", values: new List<object>(){41}),
                                        new TimeSeriesAggregateCategory(label: "Reject", values: new List<object>(){42})
                                    },
                                    defaultCategory: new TimeSeriesDefaultCategory("Others"),
                                    interpolation: new Interpolation(kind: "Step", boundary: new InterpolationBoundary(TimeSpan.FromMinutes(1)))),
                                ["Count_Aggregate"] = new AggregateVariable(
                                    aggregation: new Tsx("count()"))
                            })));

                PrintResponse(queryResponse);

                continuationToken = queryResponse.ContinuationToken;
            }
            while (continuationToken != null);
        }

        private static async Task RunGetSeriesAsync()
        {
            string continuationToken;
            do
            {
                QueryResultPage queryResponse = await _client.Query.ExecuteAsync(
                    new QueryRequest(
                        getSeries: new GetSeries(
                            timeSeriesId: TimeSeriesId,
                            searchSpan: SearchSpan,
                            filter: null,
                            projectedVariables: new[] { "Value" },
                            inlineVariables: new Dictionary<string, Variable>()
                            {
                                ["Value"] = new NumericVariable(
                                    value: new Tsx("$event.data"),
                                    aggregation: new Tsx("avg($value)"))
                            })));

                PrintResponse(queryResponse);

                continuationToken = queryResponse.ContinuationToken;
            }
            while (continuationToken != null);
        }

        private static async Task RunGetEventsAsync()
        {
            string continuationToken;
            do
            {
                QueryResultPage queryResponse = await _client.Query.ExecuteAsync(
                    new QueryRequest(
                        getEvents: new GetEvents(
                            timeSeriesId: TimeSeriesId,
                            searchSpan: SearchSpan,
                            filter: null)));

                PrintResponse(queryResponse);

                continuationToken = queryResponse.ContinuationToken;
            }
            while (continuationToken != null);
        }

        private static async Task RunGetEventsWithProjectedPropertiesAsync()
        {
            string continuationToken;
            do
            {
                QueryResultPage queryResponse = await _client.Query.ExecuteAsync(
                    new QueryRequest(
                        getEvents: new GetEvents(
                            timeSeriesId: TimeSeriesId,
                            searchSpan: SearchSpan,
                            filter: null,
                            projectedProperties: new List<EventProperty>() { new EventProperty("data", PropertyTypes.Double) })));

                PrintResponse(queryResponse);

                continuationToken = queryResponse.ContinuationToken;
            }
            while (continuationToken != null);
        }

        private static async Task<TimeSeriesInsightsClient> GetTimeSeriesInsightsClientAsync()
        {
            AuthenticationContext context = new AuthenticationContext($"{AzureActiveDirectoryLoginUrl}/{MicrosoftTenantId}", TokenCache.DefaultShared);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(ResourceUri, PowerShellAadClientId, RedirectUri, new PlatformParameters(PromptBehavior.Auto));

            TokenCloudCredentials tokenCloudCredentials = new TokenCloudCredentials(authenticationResult.AccessToken);
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials(tokenCloudCredentials.Token);

            TimeSeriesInsightsClient timeSeriesInsightsClient = new TimeSeriesInsightsClient(credentials: serviceClientCredentials)
            {
                EnvironmentFqdn = EnvironmentFqdn
            };
            return timeSeriesInsightsClient;
        }

        private static void PrintResponse(QueryResultPage queryResultPage)
        {
            Console.WriteLine();
            Console.WriteLine("Query result page:");
            Console.WriteLine();

            if (queryResultPage.Properties != null && queryResultPage.Timestamps != null)
            {
                Console.Write("timestamp,");
                Console.WriteLine(string.Join(",", queryResultPage.Properties.Select(v => v.Name)));
                int i = 0;
                foreach (DateTime? bodyTimestamp in queryResultPage.Timestamps)
                {
                    List<string> row = new List<string>();

                    row.Add(bodyTimestamp?.ToString("o"));
                    foreach (PropertyValues propertyValues in queryResultPage.Properties)
                    {
                        row.Add(propertyValues.Values[i] == null ? "null" : propertyValues.Values[i].ToString());
                    }

                    Console.WriteLine(string.Join(",", row));
                    i++;
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Result page is empty.");
            }
        }

        private static void PrintResponse(Availability availability)
        {
            Console.WriteLine("Availability:");

            if (availability != null)
            {

                Console.WriteLine("  IntervalSize: {0}", availability.IntervalSize);
                Console.WriteLine("  Range: From: {0}, To: {1}", availability.Range.FromProperty, availability.Range.To);
                Console.WriteLine("  Distribution:");
                foreach (KeyValuePair<string, int?> dist in availability.Distribution)
                {
                    Console.WriteLine("    {0}: {1}", dist.Key, dist.Value);
                }
            }
            else
            {
                Console.WriteLine("Environment is empty.");
                return;
            }

            Console.WriteLine();
        }

        private static void PrintResponse(EventSchema eventSchema)
        {
            Console.WriteLine("EventSchema");
            foreach (EventProperty eventProperty in eventSchema.Properties)
            {
                Console.WriteLine("  {0}: {1}", eventProperty.Name, eventProperty.Type);
            }

            Console.WriteLine();
        }
    }
}