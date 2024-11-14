<h2> Multiplayer Concepts </h2>

<h3> Server </h3>

- Provides a service to other computers
- Stores Information

<h3> Client </h3>

- Requests information or accesses a service from server

<h3> Host </h3>

- Server + client combined

<h3> Packets </h3>

- A small part of a larger message
- Consists of header and payload
- Header contains metadata (sender info, order of package 1/20)
- Payload contains actual part of message

<h3> Host </h3>

- Server + client combined

<h3> Transport Layer </h3>

- Collects and transmits packets
- Responsible for error correction in case of missing or corrupted packets

<h3> udp (User Data Protocol) </h3>

- fast but less reliable (Streaming)

<h3> tcp (Transmission Control Protocol)</h3>

- reliable but less fast (Downloading file)

<h3> Server Authoratative </h3>

- Server gets to make the final decision
- More consistent and secure
- Server needs to update all clients (Can be more slow/laggy)

<h3> Client Authoratative </h3>

- Client/s gets to make the final decision
- Less consistent and secure possible sync issues
- No need to wait for server

<h3> Lag & Latency </h3>

- Amount of time between a cause and it's visible effect
- Potential fix: Interpolation
