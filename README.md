## Versioning

At Trapets we use [Semantic Versioning](https://semver.org/) for all our builds.
We use a template called [Variables.yml](./PipelineTemplates/Variables.yml) to generate the different version numbers needed in the pipeline. 
The only thing you as a developer need to do is to set the Major, Minor, Patch, and PreRelease variables in the pipeline.
These variables are changed manually and committed to the git repository when the work on a new version of the software is started.
The [UpdateBuildNumber job template](./PipelineTemplates/Build/Jobs/UpdateBuildNumber.yml) updates the build number of the build when it is used.
The deliverables must also be marked with this number so that it can be easily deduced from which pipeline a deliverable (dll-files, npm, or NuGet packages) originates.

The format of the semantic version is looking like this `[Major].[Minor].[Patch]-[PreRelease].[year][month][day].[revision]`.
If the run of the pipeline is the second run for the date 2022-09-28 an example of a prerelease version would look like this `1.2.3-preview.1.220928.2`.
The prerelease version is always incremented for each run of a pipeline even if no code changes are made.
This is important because some of the pipelines create packages that are being pushed to a package feed (npm or NuGet). 
These package feeds are immutable and a package with a certain version that has been pushed to a feed can not be pushed again. 
To update the package a new version needs to be pushed to the feed.
Therefore it is very important that the version number for two runs of the same pipeline is never the same! 
The revision number makes sure that the version number is always incremented for each run of the pipeline.

If the pipeline produces NuGet or Npm packages, think about the sorting order in the feed when choosing the names of the PreRelease version.
If the need to release a `preview` or `rc` version of a package to another team a version number for the PreRelease version is recommended so that several PreRelease versions can be shared with the other team.
The recommended PreRelease versions are `preview.[number]` or `rc.[number]` because then the packages will be sorted in the feed with the newer package `rc.1` sorted before the older `preview.3`. 

A release build is always a manually triggered pipeline for a branch named `release/*`. 
When a release is built the prerelease version is removed and only the `[Major].[Minor].[Patch]` is used.
An example of the same build as before but now building from a `release/*` branch would look like this `1.2.3`.

To support tools like [SonarCloud](https://www.sonarsource.com/products/sonarcloud/) we need to build a release version of our software from a long-lived branch to be able to save the scanning result for the particular version in the tool. 
You can read more about why we are building releases of our software from a `release/*` branch in the [Branch Strategy](#branch-strategy).

### Variables.yml

We need some variables to be able to build different types of projects. 
We have collected these in the file [Variables.yml](./PipelineTemplates/Variables.yml).

```yaml
variables:
  - template: PipelineTemplates/Variables.yml
    parameters:
      Major: 1
      Minor: 2
      Patch: 3
      PreRelease: "Preview.4"
      ${{ if contains(variables['Build.SourceBranch'], 'release') }}:
        IsRelease: 'true'
      ${{ if not(contains(variables['Build.SourceBranch'], 'release')) }}:
        IsRelease: 'false'
```

Parameters:

  - **Major**  
    Increment when incompatible API changes are made.
    When this number is incremented Minor and Patch MUST be set to 0.
    Major version zero (0.y.z) is for initial development. Anything MAY change at any time. The public API SHOULD NOT be considered stable.  
    
    *Example*: 1

  - **Minor**  
    Increment when functionality in a backward-compatible manner is made.
    When this number is incremented Patch MUST be set to 0.  
    
    *Example*: 2

  - **Patch**  
    Increment when backward-compatible bug fixes are made. 
    
    *Example*: 3

  - **PreRelease**  
    Additional labels for pre-release.  
    
    *Example*: preview.1, rc.1

  - **IsRelease**  
    Should be set to `true` when building a release.
    
    *Example*: true, false

Variables created:

- **SemanticVersion.Version**  
  The version `[Major].[Minor].[Patch]`.  

  *Example*: 1.2.3

- **SemanticVersion.Revision**  
  The revision of the build `[Revision]`. 
  Increment by one on every new build for a specific day. 
  Will always start on the value `1` each day.

  *Example*: 1
  
- **SemanticVersion.Date**  
  The two last digits of the year, the month, and the day [yyyyMMdd].

  *Example*: 220928

- **SemanticVersion.Build**  
  Concatenating `[SemanticVersion.Date].[Semantic Version.Revision]`.

  *Example*: 220928.1

- **SemanticVersion.PreRelease**  
  If the pipeline is scheduled, it will be equal to 'nightly' otherwise it will get the PreRelease parameter value.

  *Example*: nightly, preview.1, rc.1

- **SemanticVersion.Suffix**  
  If the pipeline builds a release, it will be equal to '' (empty string) otherwise it will get the `[SemanticVersion.PreRelease].[SemanticVersion.Build]`.

  *Example*: '' (empty string), preview.1.220928.1, rc.1.220928.1

- **SemanticVersion.SemanticVersion**  
  If the pipeline builds a release, it will be equal to `[SemanticVersion.Version]` otherwise it will get the `[SemanticVersion.Version]-[SemanticVersion.Suffix]`.

  *Example*: 1.2.3, 1.2.3-Preview.1.220928.1, 1.2.3-rc.1.220928.1