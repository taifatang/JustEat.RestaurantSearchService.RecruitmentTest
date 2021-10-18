1. How long did you spend on the coding test? What would you add to your solution if you had more time? If you didn't spend much time on the coding test then use this as an opportunity to explain what you would add.

I have spent approximately 4.5 hours with the majority of the time spent on testing. I would add the following if I have more time
* Outcode validation, so we don't pass raw value to our third party provider
* Better swagger documentation
* Fix some of my spelling mistakes `Restaurant -> Restaurants` 
* A more defined physical boundary between `JustEatService` and `JustEathttpClient`. i.e arranging folders
* Use request builders in tests to improve readability  and reduce variable persistency at the fixture layer
* Do E2E test

2. What was the most useful feature that was added to the latest version of your chosen language? Please include a snippet of code that shows how you've used it.

I quite like the `FakeHttpClient` with `FakeHttpMessageHandler` implementation, which could be easily shared across Unit and InMemory tests.

3. How would you track down a performance issue in production? Have you ever had to do this?

I have used JetBrain DotMemory to trace memory leakages and New Relic APM for performance issues.

    For Memory Leak:
    
    1. Take 1 production box out of load balancer but live the service running
    2. Take a memory dump of the offline instance in the production box     
    3. Take a memory dump of a fresh instance of the service on staging
    4. Perform a diff of the two memory dump for preliminary analysis

    5. Host the services locally inside DotMemory and introduce traffic gradually via Postman runner (linear) or Fidler (Parallel)
    6. Take a snapshot at a specific interval after a manual garbage collection
    7. Identity objects not garbage collected
    8. Identity objects gradually increasing
    9. Check the code and confirm with Google

4. How would you improve the Just Eat APIs that you just used?

According to the Just Eat Open Api document, an InternalServerError would return a stacktrace to the consumer. I would prefer to return a more generic message containing, `errorcode`, `errorMessage`, `retryable?` and maybe a `traceId` and log the stacktrace out. 
```
500
{
"ExceptionMessage": "Object reference not set to an instance of an object.",
"Message": "An error has occurred.",
"ExceptionType": "System.NullReferenceException",
"StackTrace": "   at JE.SearchOrchestrator.Controllers.Filters.CacheControlFilter.OnActionExecuted(HttpActionExecutedContext actionExecutedContext) in \\\\Mac\\Home\\Documents\\GitHub\\SearchOrchestrator\\src\\JE.SearchOrchestrator\\Controllers\\Filters\\CacheControlFilter.cs:line 18\n   at System.Web.Http.Filters.ActionFilterAttribute.OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)\n--- End of stack"
}
```

It may be a good idea to combine other restaurant search endpoints `bypostcode`, `bylatlong` etc. They behave exactly the same and it would be less maintenance for both the consumer and developer.
