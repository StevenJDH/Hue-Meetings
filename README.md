# Hue Meetings

[![build](https://github.com/StevenJDH/Hue-Meetings/actions/workflows/dotnet-sonar-workflow.yml/badge.svg?branch=main)](https://github.com/StevenJDH/Hue-Meetings/actions/workflows/dotnet-sonar-workflow.yml)
![GitHub All Releases](https://img.shields.io/github/downloads/StevenJDH/Hue-Meetings/total)
![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/StevenJDH/Hue-Meetings?include_prereleases)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/ef02e3c5d4e845159f03a3fc621b3395)](https://app.codacy.com/gh/StevenJDH/Hue-Meetings/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=StevenJDH_Hue-Meetings&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=StevenJDH_Hue-Meetings)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=StevenJDH_Hue-Meetings&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=StevenJDH_Hue-Meetings)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=StevenJDH_Hue-Meetings&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=StevenJDH_Hue-Meetings)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=StevenJDH_Hue-Meetings&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=StevenJDH_Hue-Meetings)
[![Sonar Violations (long format)](https://img.shields.io/sonar/violations/StevenJDH_Hue-Meetings?format=long&server=https%3A%2F%2Fsonarcloud.io)](https://sonarcloud.io/dashboard?id=StevenJDH_Hue-Meetings)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=StevenJDH_Hue-Meetings&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=StevenJDH_Hue-Meetings)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=StevenJDH_Hue-Meetings&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=StevenJDH_Hue-Meetings)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=StevenJDH_Hue-Meetings&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=StevenJDH_Hue-Meetings)
![Maintenance](https://img.shields.io/badge/yes-4FCA21?label=maintained&style=flat)
![GitHub](https://img.shields.io/github/license/StevenJDH/Hue-Meetings)

Hue Meetings is a tool that was developed to help make working from home a little easier by using Philips Hue smart lights as a sort of meeting status indicator. When a call starts, the chosen light will turn red, and when the call ends, the light will either turn green or turn off. And yes, all colors can be customized. Hue Meetings also has a VPN mode to support scenarios where a direct connection to the light is not an option because network traffic is being routed over a VPN connection. 

> [!IMPORTANT]  
> Development is still ongoing, and there is still much to be done, but a first release will be available as soon as time permits.

[![Buy me a coffee](https://img.shields.io/static/v1?label=Buy%20me%20a&message=coffee&color=important&style=flat&logo=buy-me-a-coffee&logoColor=white)](https://www.buymeacoffee.com/stevenjdh)

Releases: [https://github.com/StevenJDH/Hue-Meetings/releases](https://github.com/StevenJDH/Hue-Meetings/releases)

## Features
* Supports Philips Hue API v2 where available. Latest flavor of AP1 v1 is used only for Bridge management.
* Remote API support using OAuth2 with PKCE and Digest Authentication for VPN compatibility.
* Bridge user/app key management.
* Custom color selection for meeting status.
* Supports all internal and external USB connected microphones.
* Works with any conferencing software like Teams, Skype, Zoom, Google Meet, etc. 
* High DPI Support.

## Prerequisites
* .NET 8 or newer installed.
* Any Philips Hue light.
* Hue bridge (the square one) with at least firmware 1948086000.
* Philips Hue developer account (it's free).

## Disclaimer
Hue Meetings is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

## Contributing
Thanks for your interest in contributing! There are many ways to contribute to this project. Get started [here](https://github.com/StevenJDH/.github/blob/main/docs/CONTRIBUTING.md).

## Do you have any questions?
Many commonly asked questions are answered in the FAQ:
[https://github.com/StevenJDH/Hue-Meetings/wiki/FAQ](https://github.com/StevenJDH/Hue-Meetings/wiki/FAQ)

## Want to show your support?

|Method          | Address                                                                                   |
|---------------:|:------------------------------------------------------------------------------------------|
|PayPal:         | [https://www.paypal.me/stevenjdh](https://www.paypal.me/stevenjdh "Steven's Paypal Page") |
|Cryptocurrency: | [Supported options](https://github.com/StevenJDH/StevenJDH/wiki/Donate-Cryptocurrency)    |


// Steven Jenkins De Haro ("StevenJDH" on GitHub)
