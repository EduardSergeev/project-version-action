# Project version action

Sets project version to `YYYY.MM.DD.BUILD` where:
- `YYYY.MM.DD` is the current action runner date
- `BUILD` is Github run number (unique number incremented on every build)

Resulting project version is unique but also meaningful since it also contains the date when the project was build.  
Example of resulting version: `2021.10.17.123`.  

This format of versioning is better suited for projects with continious delivery model of development: when releases are frequient and with relatively small incremental changes in between. For this type of projects [semver](https://semver.org) does not make much sense while the actual date in version format does.

## Input parameters

- `project-file`: (optional) path to the file which contains the version of the project  
  Defaults to: `package.json`
- `version-stub`: (optional) stub version value  
  Defaults to: `'0.0.0'`
- `leading-zeros`: (optional) pad month and day with `0` for a single-digit values  
  Defaults to `true` e.g `2021/06/07` current date by default results in `2021.06.07.123` version and otherwise in `2021.6.7.123`
- `time-zone`: (optional) time zone to be used to determine the current date/time  
  If not set the action runner current date/time is used 


## Output parameters

- `project-version`: is set to the resulting version and can be referenced in subsequent steps via `steps.step_id.outputs.project-version`
- `VERSION`: (environment variable) is also set to resulting project version, is set for subsequent steps and has shorter syntax `${{ env.VERSION }}`

## Usage

In your project set version to some predefined stub version value. This value should probably still be a valid version number if you intend to compile your project outside Github action. For example in Node project `package.json` by default is expected to contain stub version `0.0.0`:
```json
{
  "name": "example",
  "version": "0.0.0"
}
```

Add `EduardSergeev/project-version-action@v1` to your CI script after `actions/checkout@v2` step:

```yml
    - name: Set project version for Node project
      uses: EduardSergeev/project-version-action@main
```

If a different from `package.json` file is used or a different from `0.0.0` stub version value was specified set `project-file` and `version-stub` input parameters accordingly. For example for .NET project it could be:

```yml
    - name: Set project version for .NET project
      uses: EduardSergeev/project-version-action@main
      with:
        version-file: example.csproj
        version-stub: '65534.65534.65534.65534'
```

By default this action pads single-digit "month" and "day" bits of the resulting version with `0` but since some build systems, like Cabal does not allow leading zeros in version parts use `leading-zeroes: false` to prevent the padding:

```yml
    - name: Set project version for Cabal project
      uses: EduardSergeev/project-version-action@main
      with:
        version-file: example.cabal
        leading-zeros: false
```

Since this action uses action runner's local date/time, which is by default is UTC, the resulting version might be one day before or after your local date, depending on your local time zone. To avoid this situation you can either set runner's time zone to your local time zone, e.g.:

```yml
jobs:
  build:
    runs-on: ubuntu-latest
    env:
      TZ: Australia/Sydney
```

or set input parameter `time-zone` to your local time zone, e.g.:

```yml
    - name: Set project version for Node project
      uses: EduardSergeev/project-version-action@main
      with:
        time-zone: Australia/Sydney
```

The former will change time zone for entire action job while the latter will use it only to calculate the version without changing it for the runner.
