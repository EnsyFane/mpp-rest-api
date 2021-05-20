package utils;

import com.fasterxml.jackson.core.JsonParser;
import com.fasterxml.jackson.databind.DeserializationContext;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.deser.std.StdDeserializer;
import models.Match;
import models.MatchType;

import java.io.IOException;

public class MatchDeserializer extends StdDeserializer<Match> {
    public MatchDeserializer() {
        this(null);
    }

    public MatchDeserializer(Class<?> vc) {
        super(vc);
    }

    @Override
    public Match deserialize(JsonParser p, DeserializationContext ctxt) throws IOException {
        var match = new Match();
        var codec = p.getCodec();
        JsonNode node = codec.readTree(p);

        JsonNode idNode = node.get("id");
        var id = idNode.asInt();
        match.setId(id);

        JsonNode homeTeamNode = node.get("homeTeam");
        var homeTeam = homeTeamNode.asText();
        match.setHomeTeam(homeTeam);

        JsonNode guestTeamNode = node.get("guestTeam");
        var guestTeam = guestTeamNode.asText();
        match.setGuestTeam(guestTeam);

        JsonNode matchTypeNode = node.get("matchType");
        var matchType = matchTypeNode.asText();
        match.setMatchType(MatchType.valueOf(matchType));

        JsonNode availableSeatsNode = node.get("availableSeats");
        var availableSeats = availableSeatsNode.asInt();
        match.setAvailableSeats(availableSeats);

        JsonNode ticketPriceNode = node.get("ticketPrice");
        var ticketPrice = ticketPriceNode.asDouble();
        match.setTicketPrice((float) ticketPrice);

        return match;
    }
}
