var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// app.Run(async context => {
//     await context.Response.WriteAsync("<h1>Hello world!</h1>");
// });

// app.Use(async (context, next) => {
//     var logger = app.Services.GetRequiredService<ILogger<Program>>();
//     logger.LogInformation($"Request Host: {context.Request.Host}");
//     logger.LogInformation("My Middleware - Before");
//     await next(context);
//     logger.LogInformation("My Middleware - After");
//     logger.LogInformation($"Response StatusCode: {context.Response.StatusCode}");
// });

// This is to show how to use the `next()` method to call the next middleware in the pipeline.
// app.Use(async (context, next) =>
// {
//    var logger = app.Services.GetRequiredService<ILogger<Program>>();
//    logger.LogInformation($"ClientName HttpHeader in Middleware 1: {context.Request.Headers["ClientName"]}");
//    logger.LogInformation($"Add a ClientName HttpHeader in Middleware 1");
//    context.Request.Headers.TryAdd("ClientName", "Windows");
//    logger.LogInformation("My Middleware 1 - Before");
//    await next(context);
//    logger.LogInformation("My Middleware 1 - After");
//    logger.LogInformation($"Response StatusCode in Middleware 1: {context.Response.StatusCode}");
// });

// app.Use(async (context, next) =>
// {
//    var logger = app.Services.GetRequiredService<ILogger<Program>>();
//    logger.LogInformation($"ClientName HttpHeader in Middleware 2: {context.Request.Headers["ClientName"]}");
//    logger.LogInformation("My Middleware 2 - Before");
//    context.Response.StatusCode = StatusCodes.Status202Accepted;
//    await next(context);
//    logger.LogInformation("My Middleware 2 - After");
//    logger.LogInformation($"Response StatusCode in Middleware 2: {context.Response.StatusCode}");
// });

// This is an example showing how to combine multiple middleware components into a single middleware component.
app.Map("/lottery", app =>
{
   var random = new Random();
   var luckyNumber = random.Next(1, 6);
   app.UseWhen(context => context.Request.QueryString.Value == $"?{luckyNumber.ToString()}", app =>
   {
       app.Run(async context =>
       {
           await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
       });
   });
   app.UseWhen(context => string.IsNullOrWhiteSpace(context.Request.QueryString.Value), app =>
   {
       app.Use(async (context, next) =>
       {
           var number = random.Next(1, 6);
           context.Request.Headers.TryAdd("number", number.ToString());
           await next(context);
       });
       app.MapWhen(context => context.Request.Headers["number"] == luckyNumber.ToString(), app =>
       {
           app.Run(async context =>
           {
               await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
           });
       });
   });
   app.Run(async context =>
   {
       var number = "";
       if (context.Request.QueryString.HasValue)
       {
           number = context.Request.QueryString.Value?.Replace("?", "");
       }
       else
       {
           number = context.Request.Headers["number"];
       }
       await context.Response.WriteAsync($"Your number is {number}. Try again!");
   });
});
app.Run(async context =>
{
   await context.Response.WriteAsync($"Use the /lottery URL to play. You can choose your number with the format /lottery?1.");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
