﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
using System.Collections.Generic;
using System;
// 
// This source code was auto-generated by xsd, Version=4.0.30319.17929.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class variantGroup {
    
    private string namesField;
    
    private string gCardinalityField;
    
    private HashSet<variant> variantField;
    
    /// <remarks/>
    public string names {
        get {
            return this.namesField;
        }
        set {
            this.namesField = value;
        }
    }
    
    /// <remarks/>
    public string gCardinality {
        get {
            return this.gCardinalityField;
        }
        set {
            this.gCardinalityField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("variant")]
    public HashSet<variant> variants {
        get {
            return this.variantField;
        }
        set {
            this.variantField = value;
        }
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        variantGroup lVariantGroup = (variantGroup)obj;
        return (this.names == lVariantGroup.names && this.variants == lVariantGroup.variants);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return (this.names.GetHashCode() * 2 + this.variants.GetHashCode());
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class variant
{

    private string namesField;

    /// <remarks/>
    public string names
    {
        get
        {
            return this.namesField;
        }
        set
        {
            this.namesField = value;
        }
    }
    
    // override object.Equals
    public override bool Equals (object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType()) 
            return false;

        variant lVariant = (variant)obj;
        return (this.names == lVariant.names);
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return this.names.GetHashCode() * 3;
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class part {

    private string namesField;
    
    /// <remarks/>
    public string names {
        get {
            return this.namesField;
        }
        set {
            this.namesField = value;
        }
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        part lPart = (part)obj;
        return this.names == lPart.names;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return this.names.GetHashCode() * 5;
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class operation
{

    private string namesField;

    private HashSet<string> preconditionField;

    private HashSet<string> requirementsField;

    /// <remarks/>
    public string names
    {
        get
        {
            return this.namesField;
        }
        set
        {
            this.namesField = value;
        }
    }

    /// <remarks/>
    public HashSet<string> precondition
    {
        get
        {
            return this.preconditionField;
        }
        set
        {
            this.preconditionField = value;
        }
    }

    /// <remarks/>
    public HashSet<string> requirements
    {
        get
        {
            return this.requirementsField;
        }
        set
        {
            this.requirementsField = value;
        }
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        operation lOperation = (operation)obj;
        return this.names == lOperation.names;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return this.names.GetHashCode() * 4;
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class partOperations
{
    private string PartExpr;
    private HashSet<operation> Operations;

    public string getPartExpr()
    {
        return PartExpr;
    }

    public void setPartExpr(string pPartExpr)
    {
        PartExpr = pPartExpr;
    }

    public HashSet<operation> getOperations()
    {
        return Operations;
    }

    public void setOperations(HashSet<operation> pOperations)
    {
        Operations = pOperations;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        partOperations lPartOperations = (partOperations)obj;
        return (this.PartExpr == lPartOperations.PartExpr && this.Operations == lPartOperations.Operations);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return (this.PartExpr.GetHashCode() * 6 + this.Operations.GetHashCode());
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class variantOperations
{
    private string VariantExpr;
    private HashSet<operation> Operations;

    public string getVariantExpr()
    {
        return VariantExpr;
    }

    public void setVariantExpr(string pVariantExpr)
    {
        VariantExpr = pVariantExpr;
    }

    public HashSet<operation> getOperations()
    {
        return Operations;
    }

    public void setOperations(HashSet<operation> pOperations)
    {
        Operations = pOperations;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        variantOperations lVariantOperations = (variantOperations)obj;
        return (this.VariantExpr == lVariantOperations.VariantExpr && this.Operations == lVariantOperations.Operations);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return (this.VariantExpr.GetHashCode() * 12 + this.Operations.GetHashCode());
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class itemUsageRule
{
    private variant Variant;
    private HashSet<part> Parts;

    public variant getVariant()
    {
        return Variant;
    }

    public void setVariant(variant pVariant)
    {
        Variant = pVariant;
    }

    public HashSet<part> getParts()
    {
        return Parts;
    }

    public void setParts(HashSet<part> pParts)
    {
        Parts = pParts;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        itemUsageRule lItemUsageRule = (itemUsageRule)obj;
        return (this.Variant == lItemUsageRule.Variant && this.Parts == lItemUsageRule.Parts);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return (this.Variant.GetHashCode() * 8 + this.Parts.GetHashCode());
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class resource
{

    private string namesField;

    private HashSet<Tuple<string,string,string>> attributesField;

    private HashSet<trait> traitsField;

    /// <remarks/>
    public string names
    {
        get
        {
            return this.namesField;
        }
        set
        {
            this.namesField = value;
        }
    }

    public HashSet<Tuple<string,string,string>> attributes
    {
        get
        {
            return this.attributesField;
        }
        set
        {
            this.attributesField = value;
        }
    }

    public HashSet<trait> traits 
    {
        get 
        {
            return this.traitsField;
        }
        set 
        {
            this.traitsField = value;
        }
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        resource lResource = (resource)obj;
        return (this.names == lResource.names);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return (this.names.GetHashCode() * 9);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class trait
{
    private string namesField;
    private HashSet<trait> inheritField;
    private HashSet<Tuple<string,string>> attributesField;

    /// <remarks/>
    public string names
    {
        get
        {
            return this.namesField;
        }
        set
        {
            this.namesField = value;
        }
    }

    public HashSet<Tuple<string,string>> attributes
    {
        get
        {
            return this.attributesField;
        }
        set
        {
            this.attributesField = value;
        }
    }

    public HashSet<trait> inherit
    {
        get
        {
            return this.inheritField;
        }
        set
        {
            this.inheritField = value;
        }
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        trait lTrait = (trait)obj;
        return (this.names == lTrait.names);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return (this.names.GetHashCode() * 7);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class virtualPart2PartExpr
{
    private part virtualPart;
    private string partExpr;

    public part getVirtualPart()
    {
        return virtualPart;
    }

    public void setVirtualPart(part pVirtualPart)
    {
        virtualPart = pVirtualPart;
    }

    public string getPartExpr()
    {
        return partExpr;
    }

    public void setPartExpr(string pPartExpr)
    {
        partExpr = pPartExpr;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        virtualPart2PartExpr lVirtualPart2PartExpr = (virtualPart2PartExpr)obj;
        return (this.virtualPart == lVirtualPart2PartExpr.virtualPart && this.partExpr == lVirtualPart2PartExpr.partExpr);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return (this.partExpr.GetHashCode() * 10 + this.virtualPart.GetHashCode());
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class virtualVariant2VariantExpr
{
    private variant virtualVariant;
    private string variantExpr;

    public variant getVirtualVariant()
    {
        return virtualVariant;
    }

    public void setVirtualVariant(variant pVirtualVariant)
    {
        virtualVariant = pVirtualVariant;
    }

    public string getVariantExpr()
    {
        return variantExpr;
    }

    public void setVariantExpr(string pVariantExpr)
    {
        variantExpr = pVariantExpr;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        virtualVariant2VariantExpr lVirtualVariant2VariantExpr = (virtualVariant2VariantExpr)obj;
        return (this.virtualVariant == lVirtualVariant2VariantExpr.virtualVariant && this.variantExpr == lVirtualVariant2VariantExpr.variantExpr);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return (this.variantExpr.GetHashCode() * 11 + this.virtualVariant.GetHashCode());
    }
}




