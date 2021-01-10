export class ListElement {
    id: string | number | null = '';
    name: string = '';

    constructor(id: string | number | null, name: string) {
        this.id = id;
        this.name = name;
    }
}
