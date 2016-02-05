using System;
using System.Globalization;

namespace Escc.Gritting
{
    /// <summary>
    /// An decision from the highways team about the action to take for a specific route
    /// </summary>
    public abstract class GrittingDecision
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrittingDecision"/> class.
        /// </summary>
        protected GrittingDecision()
        {
            // set defaults
            this.MessageType = "Action proposed";
        }

        /// <summary>
        /// Gets or sets the decision id.
        /// </summary>
        /// <value>The decision id.</value>
        public int DecisionId { get; set; }

        /// <summary>
        /// The id used by the software supplier for this decision
        /// </summary>
        public string SupplierDecisionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the when the decision was made.
        /// </summary>
        /// <value>The decision time.</value>
        public DateTime DecisionTime { get; set; }

        /// <summary>
        /// Gets or sets the action as logged by highways. Includes more detail than <see cref="Action"/>.
        /// </summary>
        /// <value>The original action.</value>
        public string OriginalAction { get; set; }

        /// <summary>
        /// Gets or sets the action to be taken, as displayed on website. More detail is available in <see cref="OriginalAction"/>.
        /// </summary>
        /// <value>The action.</value>
        public string Action
        {
            get
            {
                // If plain english text not set yet, set it
                if (String.IsNullOrEmpty(this.action) && !String.IsNullOrEmpty(this.OriginalAction))
                {
                    if (GritGramsPerSquareMetre > 0 && !Plough)
                    {
                        this.action = "Gritting";
                    }
                    else if (GritGramsPerSquareMetre > 0)
                    {
                        this.action = "Gritting and snowploughs";
                    }
                    else if (Plough)
                    {
                        this.action = "Snowploughs";
                    }
                    else
                    {
                        this.action = this.OriginalAction.ToLower(CultureInfo.CurrentCulture);
                        if (this.action.Length > 1)
                        {
                            this.action = this.action.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + this.action.Substring(1);
                        }
                    }
                }

                return this.action;
            }
            set { this.action = value; }
        }
        private string action;

        /// <summary>
        /// Gets or sets whether this <see cref="GrittingDecision"/> includes a decision to plough.
        /// </summary>
        /// <value><c>true</c> to plough; otherwise, <c>false</c>.</value>
        public bool Plough { get; set; }

        /// <summary>
        /// Gets or sets the grit density in grams per square metre.
        /// </summary>
        /// <value>The grit density grams per square metre.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Metre")]
        public int GritGramsPerSquareMetre { get; set; }

        /// <summary>
        /// Gets or sets the time the proposed action will be carried out.
        /// </summary>
        /// <value>The action time.</value>
        public DateTime? ActionTime { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }
    }
}
