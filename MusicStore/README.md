# SteelToe MusicStore Sample Application
This repo tree contains a sample app illustrating how to use all of the SteelToe components together in a ASP.NET Core application. This application is based on the ASP.NET Core reference app [MusicStore](https://github.com/aspnet/MusicStore) provided by Microsoft.

In creating this application, we took the Microsoft reference application and broke it up into multiple independent services:
* MusicStoreService - provides a RESTful API to the MusicStore and its backend Music database.
* OrderService - provides a RESTful API for Order processing and its backend Order database.
* ShoppingCartService - provides a RESTful api to a ShoppingCart service and it backend ShoppingCart database.
* MusicStoreUI - provides the UI to the MusicStore application


# Building & Running
See the Readme for instructions on building and running each app.
