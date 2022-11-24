import React, { useState } from 'react';
import { Popover, PopoverBody } from 'reactstrap';

const LandingPopover = () => {
  const [isPopoverOpen, setIsPopoverOpen] = useState(false);

  const togglePopoverOpen = () => {
    setIsPopoverOpen(!isPopoverOpen);
  };

  return (
    <>
      <button
        id="PopoverBottom"
        onClick={togglePopoverOpen}
        className="landing__btn landing__btn--hire-us"
        type="button"
      >
        Hire Us
      </button>
      <Popover
        placement="bottom"
        isOpen={isPopoverOpen}
        target="PopoverBottom"
        toggle={togglePopoverOpen}
        className="landing__popover"
      >
        <PopoverBody>
          <p>We provide UX/UI and Node.JS & ReactJS development services.&ensp;
            <a
              target="_blank"
              rel="noopener noreferrer"
              href="https://aspirity.com/#uximprove"
            >
              Contact us
            </a>
            &nbsp;if you have custom development project.
          </p>
        </PopoverBody>
      </Popover>
    </>
  );
};

export default LandingPopover;
