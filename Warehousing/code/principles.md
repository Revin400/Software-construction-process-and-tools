# CargoHUB API design principles

Here we describe the design principles for the CargoHUB API code.

## Versioning

The provided API is marked as **version 1**. This means that the connected front-end dashboards (the consumers of the API) have an implicit contract with this version. To help students make the transition they can safely design a version 2 and leave the implicit contract of version 1 untouched. However, we do expect them to refactor version 1 and keep it running with minimum down time.

## Happy paths only

The system at hand only implements the **happy paths**. This means that any input that deviates from the expected situation (the implicit contract) results in a failure. The system won't crash because of a try/catch but does return a status `500 Internal Server Error`. Of course, this is of no use for efficient debugging.

The students will discover this during the reverse engineering phase when they start to write tests. We will run a set of tests to check if the students catch the exceptions.

## Incomplete functionality

The implemented API is far from complete. The connected front-end dashboards need to do a lot of work to make the system work properly. This is prone to errors so that's why the API needs to be extended to a version 2.

Here are some suggestions to make the API more reliable and usable:

- Implement paging to control the response size
- Implement filtering to filter on specific fields
- Implement rate limiting to protect the system agains rogue clients
- Implement authentication and access logging for security audits

- Identifiers must be created by the API, not by the client

- Extra endpoints to take away work for the clients:

    `/api/v2/TODO`