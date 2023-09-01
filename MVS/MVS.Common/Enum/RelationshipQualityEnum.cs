using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Common.Enum;
public enum RelationshipQuality
{
    [Description("Bonnes")]
    Good = 0,
    [Description("Conflictuelles")]
    Conflict = 1,
    [Description("Pas de relation")]
    NoRelationship = 2,
    [Description("Null")]
    NULL = 3,
}