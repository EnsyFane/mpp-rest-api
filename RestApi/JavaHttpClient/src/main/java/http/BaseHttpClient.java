package http;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.module.SimpleModule;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpDelete;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.HttpClients;

import java.io.IOException;

public abstract class BaseHttpClient<T> {
    private final String baseRestApiUrl;
    private final HttpClient client;
    protected final ObjectMapper mapper;

    public BaseHttpClient(String baseBasketballUrl, SimpleModule serializerModule) {
        baseRestApiUrl = baseBasketballUrl;
        client = HttpClients.createDefault();
        mapper = new ObjectMapper();
        mapper.registerModule(serializerModule);
    }

    protected HttpResponse Get(String route) throws IOException {
        var httpGet = new HttpGet(baseRestApiUrl + route);
        httpGet.setHeader("Accept", "application/json");
        httpGet.setHeader("Content-type", "application/json");
        return client.execute(httpGet);
    }

    protected HttpResponse Post(String route, T body) throws IOException {
        var httpPost = new HttpPost(baseRestApiUrl + route);
        var entity = new StringEntity(mapper.writeValueAsString(body));
        httpPost.setEntity(entity);
        httpPost.setHeader("Accept", "application/json");
        httpPost.setHeader("Content-type", "application/json");

        return client.execute(httpPost);
    }

    protected HttpResponse Put(String route, T body) throws IOException {
        var httpPut = new HttpPut(baseRestApiUrl + route);
        var entity = new StringEntity(mapper.writeValueAsString(body));
        httpPut.setEntity(entity);
        httpPut.setHeader("Accept", "application/json");
        httpPut.setHeader("Content-type", "application/json");

        return client.execute(httpPut);
    }

    protected HttpResponse Delete(String route) throws IOException {
        var httpDelete = new HttpDelete(baseRestApiUrl + route);
        httpDelete.setHeader("Accept", "application/json");
        httpDelete.setHeader("Content-type", "application/json");
        return client.execute(httpDelete);
    }
}
