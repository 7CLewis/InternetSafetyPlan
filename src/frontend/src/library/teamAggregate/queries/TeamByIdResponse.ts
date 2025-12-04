export class TeamByIdResponse {
  public id: string;
  public name: string;
  public description: string | null;
  public teammates: TeamByIdResponse_Teammate[];

  constructor(id: string, name: string, description: string | null, teammates: TeamByIdResponse_Teammate[]) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.teammates = teammates;
  }
}

export class TeamByIdResponse_Teammate {
  public id: string;
  public name: string;
  public dateOfBirth: string;

  constructor(id: string, name: string, dateOfBirth: string) {
    this.id = id;
    this.name = name;
    this.dateOfBirth = dateOfBirth;
  }
}