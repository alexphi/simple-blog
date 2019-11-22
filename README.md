# simple-blog
(Skill assesment test)

*Objective*

Part 1: Build a .Net web app that allows to create, edit and publish text-based blog posts, with an approval flow where two different user types may interact.

Part 2: Build a REST API to manage the posts as an Editor user. The API must expose at least two endpoints: to query the pending posts, and to approve or reject a pending post

*Project Specifics*

This project is an ASP.NET Core 3.0 website, which uses a simple json file as backing storage for posts and comments.

There is no authentication, so the two types of users are "simulated" by navigating to the `/writer` and `/editor` routes (or using the navigation bar links)

The project is built into a docker image and published to Azure as a container-based Web App.
