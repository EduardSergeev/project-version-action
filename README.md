# Project version action
[![Build Status](https://github.com/EduardSergeev/project-version-action/workflows/build/badge.svg)](https://github.com/EduardSergeev/project-version-action/actions?query=workflow%3Abuild+branch%3Amain)
[![Latest release](https://img.shields.io/github/v/release/EduardSergeev/project-version-action?label=release)](https://github.com/marketplace/actions/set-project-version)


Sets project version to `YYYY.MM.DD.BUILD` where:
- `YYYY.MM.DD` is the current action runner date
- `BUILD` is Github run number (unique number incremented on every build)

Resulting project version is unique but also meaningful since it also contains the date when the project was build.  
Example of resulting version: `2021.10.17.123`.  

This format of versioning is better suited for projects with continuous delivery model of development: when releases are frequent and with relatively small incremental changes in between. For this type of projects [semver](https://semver.org) does not make much sense while the actual date in version format does.

## Input parameters

- `project-file`: (optional) path to the file which contains the version of the project  
  Defaults to: `package.json`
- `version-stub`: (optional) stub version value  
  Defaults to: `'0.0.0.0'`
- `leading-zeros`: (optional) pad month and day with `0` for a single-digit values  
  Defaults to `false` e.g `2021/06/07` current date by default results in `2021.6.7.123` version and otherwise in `2021.06.07.123`
- `time-zone`: (optional) time zone to be used to determine the current date/time  
  If not set the action runner current date/time is used 
- `version-file-exists`: (optional) Check if specified by `version-file` parameter file does exist in repository
  Defaults to: `true`. Set it to `false` if there is no `version-file` to set version in
  e.g. when only `VERSION` environment variable is needed for build

## Output parameters

- `project-version`: is set to the resulting version and can be referenced in subsequent steps via `steps.step_id.outputs.project-version`
- `VERSION`: (environment variable) is also set to resulting project version, is set for subsequent steps and has shorter syntax `${{ env.VERSION }}`

## Usage

### Default settings

In your project set version to some predefined stub version value. This value should probably still be a valid version number if you intend to compile your project outside Github action. For example if Node project if its `package.json` contains stub version of `0.0.0.0` everything should work with default settings:
```json
{
  "name": "example",
  "version": "0.0.0.0"
}
```

### Placement in CI pipeline

Add `EduardSergeev/project-version-action@v6` to your CI script after `actions/checkout@v3` step:

```yml
    - uses: actions/checkout@v3
    - name: Set project version for Node.js project
      uses: EduardSergeev/project-version-action@v6
```

All subsequent build steps then will be able to reference current build version either via `VERSION` environment variable or via `project-version` step's output parameter.

### Input parameters

If a different from `package.json` file is used or a different from `0.0.0.0` stub version value was specified set `project-file` and `version-stub` input parameters accordingly. For example for Node.js project with default three-parts `version` format it could be:

```yml
    - name: Set project version for Node.js project
      uses: EduardSergeev/project-version-action@v6
      with:
        version-stub: '0.0.0'
```

While for .NET project with default "local" build number set to `65534.65534.65534.65534` it could be:

```yml
    - name: Set project version for .NET project
      uses: EduardSergeev/project-version-action@v6
      with:
        version-file: example.csproj
        version-stub: '65534.65534.65534.65534'
```

By default this action does not pad single-digit "month" and "day" bits of the resulting version with `0` since some build systems do not allow leading zeros in version parts. Use `leading-zeroes: true` to force the padding:

```yml
    - name: Set project version for Cabal project
      uses: EduardSergeev/project-version-action@v6
      with:
        version-file: example.cabal
        leading-zeros: true
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
      uses: EduardSergeev/project-version-action@v6
      with:
        time-zone: Australia/Sydney
```

The former will change time zone for entire action job while the latter will use it only to calculate the version without changing it for the runner.

If only `VERSION` environment variable (or `project-version` step's output parameter) is needed for a build and there is no `version-file` to set version in set `version-file-exists` input parameter to `false`, e.g.:

```yml
    - name: Set VERSION environment variable only
      uses: EduardSergeev/project-version-action@v6
      with:
        version-file-exists: false
```

### Output parameter

In addition to environment variable `VERSION` being set to the calculated current build number (which can be used in the subsequent build steps via `${{ env.VERSION }}` expression) this action also sets output parameter `project-version` to the same build number which can be read in the consequent build steps via `${{ steps.[this-action-step_id].outputs.project-version }}` expression.
