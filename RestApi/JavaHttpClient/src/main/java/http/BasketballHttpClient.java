package http;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.module.SimpleModule;
import models.Match;
import models.MatchType;
import org.apache.http.HttpResponse;
import org.apache.http.util.EntityUtils;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.List;

@SuppressWarnings("FieldCanBeLocal")
public class BasketballHttpClient extends BaseHttpClient<Match> {
    private final String addMatchRoute = "/matches";
    private final String getMatchRoute = "/matches/%d";
    private final String getMatchesRoute = "/matches";
    private final String updateMatchRoute = "/matches/%d";
    private final String deleteMatchRoute = "/matches/%d";

    public BasketballHttpClient(String baseBasketballUrl, SimpleModule module) {
        super(baseBasketballUrl, module);
    }

    public HttpMatchResponse AddMatch(Match match) {
        HttpResponse result;
        try {
            result = Post(addMatchRoute, match);
        } catch (IOException e) {
            return responseFromException(e);
        }

        return responseFromResult(result, StatusCodes.CREATED, true);
    }

    public HttpMatchResponse GetMatchById(Integer id) {
        HttpResponse result;
        try {
            result = Get(String.format(getMatchRoute, id));
        } catch (IOException e) {
            return responseFromException(e);
        }

        return responseFromResult(result, StatusCodes.OK, true);
    }

    public HttpMatchesResponse GetMatches() {
        HttpResponse result;
        try {
            result = Get(getMatchesRoute);
        } catch (IOException e) {
            return matchesResponseFromException(e);
        }

        return matchesResponseFromResult(result, StatusCodes.OK, true);
    }

    public HttpMatchResponse UpdateMatch(Integer id, Match updatedMatch) {
        HttpResponse result;
        try {
            result = Put(String.format(updateMatchRoute, id), updatedMatch);
        } catch (IOException e) {
            return responseFromException(e);
        }

        return responseFromResult(result, StatusCodes.NO_CONTENT, false);
    }

    public HttpMatchResponse DeleteMatch(Integer id) {
        HttpResponse result;
        try {
            result = Delete(String.format(deleteMatchRoute, id));
        } catch (IOException e) {
            return responseFromException(e);
        }

        return responseFromResult(result, StatusCodes.NO_CONTENT, false);
    }

    private HttpMatchResponse responseFromException(Exception e) {
        var response = new HttpMatchResponse();
        response.setSuccess(false);
        response.setStatusCode(400);
        response.setErrorMessage(e.toString());
        return response;
    }

    private HttpMatchesResponse matchesResponseFromException(Exception e) {
        var response = new HttpMatchesResponse();
        response.setSuccess(false);
        response.setStatusCode(400);
        response.setErrorMessage(e.toString());
        return response;
    }

    private HttpMatchResponse responseFromResult(HttpResponse result, Integer expectedCode, Boolean hasBody) {
        var response = new HttpMatchResponse();
        response.setStatusCode(result.getStatusLine().getStatusCode());
        response.setSuccess(result.getStatusLine().getStatusCode() == expectedCode);
        if (response.getSuccess()) {
            if (hasBody) {
                try {
                    var jsonMatch = EntityUtils.toString(result.getEntity(), StandardCharsets.UTF_8);
                    var match = mapper.readValue(jsonMatch, Match.class);
                    response.setMatch(match);
                    return response;
                } catch (IOException e) {
                    response.setErrorMessage("Could not read response body.");
                    return response;
                }
            }

            return response;
        } else {
            response.setErrorMessage(result.getEntity().toString());
        }

        return response;
    }

    private HttpMatchesResponse matchesResponseFromResult(HttpResponse result, Integer expectedCode, Boolean hasBody) {
        var response = new HttpMatchesResponse();
        response.setStatusCode(result.getStatusLine().getStatusCode());
        response.setSuccess(result.getStatusLine().getStatusCode() == expectedCode);
        if (response.getSuccess()) {
            if (hasBody) {
                try {
                    var jsonMatch = EntityUtils.toString(result.getEntity(), StandardCharsets.UTF_8);
                    var match = mapper.readValue(jsonMatch, new TypeReference<List<Match>>() {});
                    response.setMatches(match);
                    return response;
                } catch (IOException e) {
                    response.setErrorMessage("Could not read response body.");
                    return response;
                }
            }

            return response;
        } else {
            response.setErrorMessage(result.getEntity().toString());
        }

        return response;
    }
}
