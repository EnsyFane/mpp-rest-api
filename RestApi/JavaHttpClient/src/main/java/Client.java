import com.fasterxml.jackson.core.Version;
import com.fasterxml.jackson.databind.module.SimpleModule;
import http.BasketballHttpClient;
import models.Match;
import models.MatchType;
import utils.MatchDeserializer;
import utils.MatchSerializer;

import java.io.IOException;
import java.util.Properties;

public class Client {

    public static void main(String[] args) {
        var props = new Properties();
        try {
            var loader = Thread.currentThread().getContextClassLoader();
            props.load(loader.getResourceAsStream("app.config"));
        } catch (IOException e) {
            e.printStackTrace();
        }

        var module = new SimpleModule("MatchSerializer", new Version(1, 0, 0, null, null, null));
        module.addSerializer(Match.class, new MatchSerializer());
        module.addDeserializer(Match.class, new MatchDeserializer());
        var httpClient = new BasketballHttpClient(props.getProperty("BasketballBaseUrl"), module);

        var match = new Match();
        match.setHomeTeam("My team");
        match.setGuestTeam("Your team");
        match.setAvailableSeats(100);
        match.setMatchType(MatchType.Qualifying);
        match.setTicketPrice(1.2f);

        // Add match
        System.out.println("Adding match");
        System.out.println(match);
        var addedMatchResponse = httpClient.AddMatch(match);
        if (!addedMatchResponse.getSuccess()) {
            System.out.println("Match not added.");
            System.out.println(addedMatchResponse.getStatusCode() + ":" + addedMatchResponse.getErrorMessage());
            return;
        }
        var addedMatch = addedMatchResponse.getMatch();
        System.out.println("Match added.");
        System.out.println(addedMatchResponse.getMatch());
        System.out.println();

        // Get match
        System.out.println("Getting added match");
        System.out.println(addedMatch);
        var getMatchResponse = httpClient.GetMatchById(addedMatch.getId());
        if (!getMatchResponse.getSuccess()) {
            System.out.println("Match get failed.");
            System.out.println(getMatchResponse.getStatusCode() + ":" + getMatchResponse.getErrorMessage());
            return;
        }
        System.out.println("Received match from REST API.");
        System.out.println(getMatchResponse.getMatch());
        System.out.println();

        if (!addedMatch.equals(getMatchResponse.getMatch())) {
            System.out.println("The received match is not the same as the add match.");
            return;
        }

        // Get all matches
        System.out.println("Getting all matches");
        var getAllMatchesResponse = httpClient.GetMatches();
        if (!getAllMatchesResponse.getSuccess()) {
            System.out.println("All matches get failed.");
            System.out.println(getAllMatchesResponse.getStatusCode() + ":" + getAllMatchesResponse.getErrorMessage());
            return;
        }
        System.out.println("Received matches from REST API");
        getAllMatchesResponse.getMatches().forEach(System.out::println);
        System.out.println();
        if (!getAllMatchesResponse.getMatches().contains(addedMatch)) {
            System.out.println("The received match list does not contain the added match.");
            return;
        }

        // Update match
        var matchUpdate = new Match();
        matchUpdate.setHomeTeam("My team");
        matchUpdate.setGuestTeam("Your team");
        matchUpdate.setAvailableSeats(100);
        matchUpdate.setMatchType(MatchType.Qualifying);
        matchUpdate.setTicketPrice(1.2f);
        System.out.println("Updating match.");
        System.out.println(matchUpdate);
        var updateMatchResponse = httpClient.UpdateMatch(addedMatch.getId(), matchUpdate);
        if (!updateMatchResponse.getSuccess()) {
            System.out.println("Update match failed.");
            System.out.println(updateMatchResponse.getStatusCode() + ":" + updateMatchResponse.getErrorMessage());
            return;
        }
        System.out.println("Match updated.");
        System.out.println();
        var updatedMatch = httpClient.GetMatchById(addedMatch.getId()).getMatch();
        System.out.println(updatedMatch);
        matchUpdate.setId(addedMatch.getId());
        if (!matchUpdate.equals(updatedMatch)) {
            System.out.println("The updated match does not match the match update.");
            return;
        }

        // Delete match
        System.out.println("Deleting match with id: " + addedMatch.getId());
        var deletedMatchResponse = httpClient.DeleteMatch(addedMatch.getId());
        if (!deletedMatchResponse.getSuccess()) {
            System.out.println("Match not deleted.");
            System.out.println(deletedMatchResponse.getStatusCode() + ":" + deletedMatchResponse.getErrorMessage());
            return;
        }
        System.out.println("Match deleted.");
        System.out.println();
        System.out.println("All tests pass.");
    }
}
