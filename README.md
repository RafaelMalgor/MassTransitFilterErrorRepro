# MassTransitFilterErrorRepro
This is a reproduction of a potential bug/issue on MassTransit filters.
## Error description
We are adding a Scoped Filter that wraps the calls with a try/catch block. On the catch block we are not re-throwing the exception,
so we expect the error to be saliently swallowed, thus preventing a Fault from being published. 

On MT 8.0.2 this is not the case, the Fault is being published anyways.

## Repo description
The project is quite small and all is done in memory.
It has two commits, the older one is using MT 7.3.0 and the filter is working as expected (silent error handling).
The latest commit is using MT 8.0.2 and the filter is not working (The Fault is being published regardless of what the filter does).
The commits are tagged with 'working' and 'not-working' to easily switch between the two states.
The project is quite small and all is done in memory.

## Why?
This is a demo of the error. On my project I don't want to just silently fail, I want the filter to do a couple of things before re-throwing the error, and potentially swallow it silently on some special cases.
