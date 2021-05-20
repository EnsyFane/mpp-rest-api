package models;

import java.util.Objects;

public class Match {
    private Integer id;
    private String homeTeam;
    private String guestTeam;
    private MatchType matchType;
    private Integer availableSeats;
    private Float TicketPrice;

    public Match() {}

    public Match(Integer id, String homeTeam, String guestTeam, MatchType matchType, Integer availableSeats, Float ticketPrice) {
        this.id = id;
        this.homeTeam = homeTeam;
        this.guestTeam = guestTeam;
        this.matchType = matchType;
        this.availableSeats = availableSeats;
        TicketPrice = ticketPrice;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Match match = (Match) o;
        return Objects.equals(getId(), match.getId()) &&
                getHomeTeam().equals(match.getHomeTeam()) &&
                getGuestTeam().equals(match.getGuestTeam()) &&
                getMatchType() == match.getMatchType() &&
                getAvailableSeats().equals(match.getAvailableSeats()) &&
                getTicketPrice().equals(match.getTicketPrice());
    }

    @Override
    public int hashCode() {
        return Objects.hash(getId(), getHomeTeam(), getGuestTeam(), getMatchType(), getAvailableSeats(), getTicketPrice());
    }

    @Override
    public String toString() {
        return "Match{" +
                "id=" + id +
                ", homeTeam='" + homeTeam + '\'' +
                ", guestTeam='" + guestTeam + '\'' +
                ", matchType=" + matchType +
                ", availableSeats=" + availableSeats +
                ", TicketPrice=" + TicketPrice +
                '}';
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getHomeTeam() {
        return homeTeam;
    }

    public void setHomeTeam(String homeTeam) {
        this.homeTeam = homeTeam;
    }

    public String getGuestTeam() {
        return guestTeam;
    }

    public void setGuestTeam(String guestTeam) {
        this.guestTeam = guestTeam;
    }

    public MatchType getMatchType() {
        return matchType;
    }

    public void setMatchType(MatchType matchType) {
        this.matchType = matchType;
    }

    public Integer getAvailableSeats() {
        return availableSeats;
    }

    public void setAvailableSeats(Integer availableSeats) {
        this.availableSeats = availableSeats;
    }

    public Float getTicketPrice() {
        return TicketPrice;
    }

    public void setTicketPrice(Float ticketPrice) {
        TicketPrice = ticketPrice;
    }
}
