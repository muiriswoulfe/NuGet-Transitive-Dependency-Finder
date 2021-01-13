# Contributing

<!-- © Muiris Woulfe. Licensed under the MIT License. -->

Contributions to this project are welcome and appreciated.

## Bug Reports

**If you find a security vulnerability, do NOT open a bug report. Instead,
follow the instructions under the [Security Policy][security].**

Bug reports are appreciated to help improve quality and resolve issues being
encountered by consumers.

Before filing a bug report, you should search the [issue tracker][bugtracker] to
see if the issue has previously been reported. If so, you should update the
existing bug report with any additional details instead of filing a new report.

If filing a [new bug report][bugreport], please provide as much information as
possible. Detailed information helps others to reproduce the issue quickly,
without requiring follow up questioning.

You should try to provide details for all appropriate headings in the bug report
template:

1. **Summary:** A summary outlining the nature of the issue.
1. **Reproduction Steps:** A set of clear steps that can be used to reproduce
   the issue on another device.
1. **Behavior:**
   1. **Actual:** The behavior that was observed after performing the
      reproduction steps.
   1. **Expected:** The behavior that was expected, instead of the actual
      behavior. It is beneficial to also include information about why the
      specified behavior was expected.
1. **Console Output:** If applicable, the output printed to the console.
1. **System Information:** Information about the system on which the issue
   occurred, including but not limited to the operating system name and version,
   .NET version, and IDE or compiler name and version.
1. **Additional Information:** Any additional information that may be relevant
   to the issue. This might include the lines of code that you have identified
   as causing the bug, and potential solutions with your opinions as to their
   merits.

## Feature Requests

Feature requests are appreciated as they help guide the development of the
project to address consumer needs.

Before filing a feature request, you should search the
[issue tracker][featuretracker] to see if the issue has previously been
reported. If so, you should update the existing feature request with any
additional details instead of filing a new request.

If filing a [new feature request][featurerequest], please provide as much
information as possible. Detailed information facilitates the rapid assessment
and implementation of new features.

You should try to provide details for all appropriate headings in the feature
request template:

1. **Summary:** A summary outlining the nature of the request.
1. **Proposed Solution:** A detailed explanation of how you believe the feature
   should behave.
1. **Alternatives Considered:** An optional set of alternative solutions that
   you considered in place of the proposed solution. These could include any
   workarounds that you are currently using, but which you hope to remove once
   the feature request has been addressed.
1. **Additional Information:** Any additional information that may be relevant
   to the request. This might include proposed code changes or a discussion as
   to the desired architecture of the solution.

## Pull Requests

Pull requests are always appreciated. If you are new to the project, consider
addressing one of the issues identified as a [good first issue][goodfirstissue].

Before creating a new PR, please consult the following list.

1. First, create a [bug report][bugreport] or [feature request][featurerequest]
   as appropriate to track the issue.
1. Ensure there is sufficient support for addressing the issue before embarking
   on a solution. Otherwise, you risk spending time working on something that
   may not be approved for merging into the project.
1. Ensure you adhere to the coding guidelines. These are not explicitly listed,
   but whenever you push to any branch of the repo, the automatically invoked
   build process will validate compliance with the coding guidelines. If you
   believe there is an implicit rule being adhered to but not enforced, please
   file a [bug report][bugreport].

By contributing to the project, you agree to license your contribution under the
[MIT License][license].

<!-- References -->

[security]:
  SECURITY.md
[bugreport]:
  https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/issues/new?assignees=muiriswoulfe&labels=bug&template=bug-report.md
[bugtracker]:
  https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/labels/bug
[featurerequest]:
  https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/issues/new?assignees=muiriswoulfe&labels=enhancement&template=feature-request.md
[featuretracker]:
  https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/labels/enhancement
[goodfirstissue]:
  https://github.com/muiriswoulfe/NuGet-Transitive-Dependency-Finder/labels/good%20first%20issue
[license]:
  ../LICENSE.md
