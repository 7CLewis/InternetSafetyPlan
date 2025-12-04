export class CreateUltimateGoalCommand {
  public teamId: string;
  public name: string;
  public description: string | null;

  constructor(teamId: string, name: string, description: string) {
    this.teamId = teamId;
    this.name = name;
    this.description = description;
  }
}