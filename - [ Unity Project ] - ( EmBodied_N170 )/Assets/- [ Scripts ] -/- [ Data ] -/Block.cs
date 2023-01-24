using System;
using System.Collections;
using System.Collections.Generic;
using Questionnaire;
using UnityEngine;

namespace Data 
{
    namespace Input
    {
        /// <summary>
        /// A class used to describe the setup and behaviour of a given block.
        /// </summary>
        [Serializable]
        public class Block
        {
            // Editor usage variable for rendering the trial setup controls within the custom editor window.
            public bool trialListVisibility;
            // A list of trial data for the block, describes what will come about in each trial of the block.
            [SerializeReference] public List<Trial> trials = new List<Trial>();
            
            // Editor usage variable for rendering the questionnaire setup controls within the custom editor window.
            public bool questionnaireListVisibility;
            // A list of objects defining what panels and questions the questionnaire will render, and what they will look like (Sliders, Buttons, Images etc.)
            [SerializeReference] public List<QuestionnairePanel> blockQuestionnairePanels = new List<QuestionnairePanel>();

            // The room the player is expected to be interactive with.
            public Enums.Room targetRoom;
        }
    }
}