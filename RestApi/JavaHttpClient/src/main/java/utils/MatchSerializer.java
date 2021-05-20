package utils;

import com.fasterxml.jackson.core.JsonGenerator;
import com.fasterxml.jackson.databind.SerializerProvider;
import com.fasterxml.jackson.databind.ser.std.StdSerializer;
import models.Match;

import java.io.IOException;

public class MatchSerializer extends StdSerializer<Match> {
    public MatchSerializer() {
        this(null);
    }

    public MatchSerializer(Class<Match> t) {
        super(t);
    }

    @Override
    public void serialize(Match match, JsonGenerator gen, SerializerProvider provider) throws IOException {
        gen.writeStartObject();
        gen.writeStringField("HomeTeam", match.getHomeTeam());
        gen.writeStringField("GuestTeam", match.getGuestTeam());
        gen.writeStringField("MatchType", match.getMatchType().toString());
        gen.writeNumberField("AvailableSeats", match.getAvailableSeats());
        gen.writeNumberField("TicketPrice", match.getTicketPrice());
        gen.writeEndObject();
    }
}
