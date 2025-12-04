export class UltimateGoalByIdResponse {
  public id: string;
  public teamId: string;
  public name: string;
  public description: string | null;

  constructor(id: string, teamId: string, name: string, description: string | null) {
    this.id = id;
    this.teamId = teamId;
    this.name = name;
    this.description = description;
  }
}