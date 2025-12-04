export class CreateEditDevice {
  public id: string;
  public teamId: string;
  public name: string;
  public nickname: string | null;
  public type: number;
  public teammateIds: string[];
  public tags: string[];

  constructor(id: string, teamId: string, name: string, nickname: string | null, type: number, teammateIds: string[], tags: string[]) {
    this.id = id;
    this.teamId = teamId;
    this.name = name;
    this.nickname = nickname;
    this.type = type;
    this.teammateIds = teammateIds;
    this.tags = tags;
  }
}