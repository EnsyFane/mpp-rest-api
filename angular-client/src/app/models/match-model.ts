export class Match {
    id?: number;
    homeTeam: string;
    guestTeam: string;
    matchType: MatchType;
    availableSeats: number;
    ticketPrice: number;
}

export enum MatchType {
    Qualifying = 'Qualifying',
    Round1 = 'Round1',
    Round2 = 'Round2',
    Round3 = 'Round3',
    Quarterfinals = 'Quarterfinals',
    Semifinals = 'Semifinals',
    Final = 'Final'
}