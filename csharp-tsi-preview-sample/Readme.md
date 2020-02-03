# Azure Time Series Insights Preview - Data Plane Client Sample App

This C# example demonstrates how to query data from the [Preview Data Access APIs](https://docs.microsoft.com/rest/api/time-series-insights/preview) in Azure Time Series Insights Preview environments as described in [Query data from the Azure Time Series Insights Preview environment using C#](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-update-query-data-csharp).

## Prerequisites 

1. [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) - Version 16.4.2+ is recommended (Visual Studio 2015+).
1. Open the `TSIPreviewDataPlaneclient.sln` solution and set `DataPlaneClientSampleApp` as the default project
1. Run the [GenerateCode.bat](https://github.com/Azure-Samples/Azure-Time-Series-Insights/blob/master/csharp-tsi-preview-sample/DataPlaneClient/GenerateCode.bat) as specified in the [Readme.md](https://github.com/Azure-Samples/Azure-Time-Series-Insights/blob/master/csharp-tsi-preview-sample/DataPlaneClient/Readme.md) to generate the Time Series Insights Preview client dependencies.
1. Compile the example to an executable `.exe` file.
1. Run the `.exe` file by double-clicking on it.

## See also

* The [Azure Time Series Insights API reference](https://docs.microsoft.com/rest/api/time-series-insights/preview) documentation for all Preview REST APIs.

* Follow the [Authentication and authorization](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-authentication-and-authorization#summary-and-best-practices) documentation to register your application in Azure Active Directory.

* The [TSI JS client SDK](https://github.com/microsoft/tsiclient/blob/master/docs/API.md).
