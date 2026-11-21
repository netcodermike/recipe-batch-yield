# Recipe Batch Yield

Rescale recipe ingredient weights and round each item to the resolution of the kitchen scale.

![Sample run from Recipe Batch Yield](docs/demo.webp)

## Run

Install .NET 8 SDK and runtime. Run these commands from the repository root.

```sh
dotnet build --nologo
dotnet bin/Debug/net8.0/App.dll sample.json
```

Replace `sample.json` with a JSON file following the same structure. The app prints a JSON report to standard output.

Each ingredient is its unique name, original weight in grams and positive scale resolution. Rounding goes to the nearest increment with midpoint ties rounded up.

## Verify

Run `python check.py` with Python 3.11 or newer after the build step when applicable. The check verifies the bundled scenario and confirms that an invalid input returns an error.
