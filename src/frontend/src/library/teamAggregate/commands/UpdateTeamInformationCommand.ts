export class UpdateTeamInformationCommand {
  public teamId: string;
  public name: string;
  public description: string | null;

  constructor(teamId: string, name: string, description: string | null) {
    this.teamId = teamId;
    this.name = name;
    this.description = description;
  }
}