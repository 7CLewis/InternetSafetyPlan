export class TeammateByIdResponse {
  public id: string;
  public name: string;
  public dateOfBirth: string | null;

  constructor(id: string, name: string, dateOfBirth: string | null) {
    this.id = id;
    this.name = name;
    this.dateOfBirth = dateOfBirth;
  }
}