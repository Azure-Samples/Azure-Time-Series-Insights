---
title: Hello World Single Page Web Application
description: 
---

# Introduction

This sample shows how to create a simple single page web application in JavaScript that uses Time Series Insights Query API and does not require any server-side code.

# Setup

This article explains how to configure a custom application that calls the Azure Time Series Insights API.

## Create Azure Active Directory Application

1. In the Azure portal, select **Azure Active Directory** > **App registrations** > **New application registration**.

2. Give the application a name, select the type to be **Web app / API**, select "http://localhost/" for **Sign-on URL**, and click **Create**.

3. Select your newly created application and copy its application ID to your favorite text editor.

4. Click on **Manifest** and change **"oauth2AllowImplicitFlow"** to **true**, click **Save**.

5. Click on **Settings**, **API Access**, **Required Permissions**, **Add**.

6. Select **Azure Time Series Insights**, check the permissions checkbox, click on **Select** and **Done** buttons.

## Setting up TSI Hello World Sample Application on IIS 
 
1. Enable IIS if not already enabled (http://www.betterhostreview.com/turn-on-iis-windows-10.html) 
2. Download sample zip file and extract to a file location (e.g. C:\TsiHelloWorldSampleApp) 
3. Create a new web site with 
- Physical path: C:\TsiHelloWorldSampleApp
- Binding Type: http
- IP address: All unassigned
- Port: 80
- Host name: localhost
4. Allow IIS AppPool\DefaultAppPool access to your sample folder (e.g. C:\TsiHelloWorldSampleApp)  
5. Edit C:\TsiHelloWorldSampleApp\index.html and populate **clientId** with application ID and &&**postLogoutRedirectUri** to be the same as in application registration.
6. Navigate to http://localhost/ in your browser.
 
## See also

* [Query API](/rest/api/time-series-insights/time-series-insights-reference-queryapi) for the full Query API reference
* [Create a service principal in the Azure portal](../azure-resource-manager/resource-group-create-service-principal-portal.md)