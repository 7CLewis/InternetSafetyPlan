export class TeamByUserEmailResponse {
  public id: string;
  public name: string;
  public description: string;
  public teammates: TeamByUserEmailResponse_Teammate[];

  constructor(id: string, name: string, description: string, teammates: TeamByUserEmailResponse_Teammate[]) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.teammates = teammates;
  }
}

export class TeamByUserEmailResponse_Teammate {
  public id: string;
  public name: string;
  public dateOfBirth: string;

  constructor(id: string, name: string, dateOfBirth: string) {
    this.id = id;
    this.name = name;
    this.dateOfBirth = dateOfBirth;
  }
}