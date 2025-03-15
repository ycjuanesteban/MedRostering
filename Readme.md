# Med Rostering

This is just a simple approach to resolve the med/nurse rostering problem.

Each doctor can have the follow restrictions for the month:

- daysOff: numeric array that represent the days the doctor cannot work.
- daysOfWeekOff: days (in spanish) that the doctor cannot work every week.
- vacations: numeric array that represent a date range of vacations.

All this request things can be set in `configuration.json` file like the follow example:

```json
{
  "month": "Diciembre",
  "daysWithTwoDoctors": [ "Lunes" ],
  "doctors": [
    {
      "name": "Juan",
      "requests": {
        "daysOfWeekOff": [
          "Martes"
        ]
      }
    },
    {
      "name": "Pedro",
      "requests": {
        "vacations": {
          "start": 1,
          "end": 20
        }
      }
    },
    {
      "name": "Maria",
      "requests": {
        "daysOff": [1, 20, 24, 25]
      }
    }
}
```

The basic configuration is like this:
- month: in spanish
- daysWithTwoDoctors: days in the week that should have two doctors
- doctors: array of all the med/nurses to process