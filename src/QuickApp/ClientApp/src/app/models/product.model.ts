export class Product
{
    public id: number
    public name: string;
    public description: string;
    public icon: string;
    public buyingPrice: number;
    public sellingPrice: number;
    public unitsInStock: number;
    public isActive: boolean;
    public isDiscontinued: boolean;
    public dateCreated: Date;
    public dateModified: Date;


    //public ParentId: number;
    //public Parent: string;

    public ProductCategoryId: number;
    public ProductCategory: number;

    public CreatedBy: string;
    public UpdatedBy: string;
    public UpdatedDate: Date;
    public CreatedDate: Date;
}