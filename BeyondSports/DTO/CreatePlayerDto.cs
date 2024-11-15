﻿using BeyondSports.Enum;
using System.ComponentModel.DataAnnotations;
using BeyondSports.Validation;

namespace BeyondSports.DTO
{
    public class CreatePlayerDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Range(1, 99)]
        public int Number { get; set; }

        [Required]
        [EnumValidation(typeof(Position))]
        public string Position { get; set; } = null!;

        [Required]
        [EnumValidation(typeof(Foot))]
        public string Foot { get; set; } = null!;

        [ValidAge(16, 50)]
        public DateOnly BirthDate { get; set; }

        [Range(120, 240)]
        public int Height { get; set; }

        public bool IsInjured { get; set; }

        [Required]
        public int TeamId { get; set; }
    }
}
