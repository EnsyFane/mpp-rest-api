export enum EventName {
    OpenSidenav = 'OPEN_SIDENAV',
    CloseSidenav = 'CLOSE_SIDENAV',
    AddMatch = 'ADD_MATCH',
    EditMatch = 'EDIT_MATCH',
    DeleteMatch = 'DELETE_MATCH',
    SidenavSecondaryAction = 'SIDENAV_SECONDARY_ACTION'
}

export class AppEvent {
    name: EventName;
    payload: any;

    constructor(name: EventName, payload?: any) {
        this.name = name;
        this.payload = payload;
    }
}