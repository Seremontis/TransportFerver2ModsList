export interface Mod {
    Title: string;
    Author: string;
    Category: string;
    CreateData: string;
    UpdateData: string;
    WebsiteSource: string;
    Image: string;
    Id: number;
    StateFile: number;
    NameWebsite: number;
}

export interface CategoriesMap {
    ParentElement: string;
    ChildElement: string;
}

export interface RootObject {
    Mods: Mod[];
    CategoriesMap: CategoriesMap[];
}