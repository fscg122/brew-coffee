# brew-coffee

A small ASP.NET Core Web API that simulates an internet-connected coffee machine.

## Endpoint

`GET /brew-coffee`

Behavior:

- Returns `200 OK` with a JSON response when coffee is available.
- Checks the current temperature using OpenWeather and returns iced coffee when it is above `30°C`.
- Returns `503 Service Unavailable` on every fifth request.
- Returns `418 I'm a teapot` on April 1, with an empty response body.

Example success response:

```json
{
  "message": "Your piping hot coffee is ready",
  "prepared": "2021-02-03T11:56:24+0900"
}
```

If the current temperature is greater than `30°C`, the message becomes:

```json
{
  "message": "Your refreshing iced coffee is ready",
  "prepared": "2021-02-03T11:56:24+0900"
}
```

## Configuration

Set the OpenWeather API settings in [`appsettings.json`](C:/Users/Franz/source/repos/brew-coffee/appsettings.json) or with environment variables:

```json
"OpenWeather": {
  "ApiKey": "71c01112014ded53838c42e4103700e8",
  "Latitude": 14.5995,
  "Longitude": 120.9842
}
```

## Run

```powershell
dotnet run
```

The API will start using the default ASP.NET Core launch settings.

## Test

```powershell
dotnet test brew-coffee.sln
```
