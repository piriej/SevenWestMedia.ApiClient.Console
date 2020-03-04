# Project Overview
The solution is delivered as a .net core 3.1, C#8.0 application.

* An executable console application 
* A library project, responsible for API abstraction
* A test project testing the library.

## 1.	The data source may change.

There is a configuration file local.settings.json. The base address

## 2.	The endpoint could go down.

There is a Polly retry policy in place to retry failed connections. (And conversely a circuitbreaker to prevent clients hitting a server that is non responsive)

## 3.	The endpoint has known to be slow in the past.

There is a timeout setting in the local.settings.json file that can be modified for slow apis.


