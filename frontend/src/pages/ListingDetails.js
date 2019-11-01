import React, { useState } from "react";
import {
  Grid,
  Typography,
  Button,
  Dialog,
  DialogContent
} from "@material-ui/core";
import useApi from "../hooks/useApi";
import ContactSellerForm from "../components/ContactSellerForm";

type Props = {
  id: string
};

const useToggle = (startOpened = false) => {
  const [toggle, setToggle] = useState(startOpened);
  const open = () => setToggle(true);
  const close = () => setToggle(false);
  return [toggle, open, close];
};

const ListingDetails = ({ id }: Props) => {
  const { data: listing } = useApi(`listings/${id}`);
  const [isOpen, open, close] = useToggle();

  return (
    <Grid container spacing={3}>
      {listing && (
        <>
          <Grid item xs={6}>
            <img src="https://www.fillmurray.com/300/300" alt="Book" />
          </Grid>
          <Grid item xs={6}>
            <Typography variant="h2">{listing.title}</Typography>
            <Typography variant="h4">{listing.description}</Typography>
            <Typography variant="h4">{listing.authors}</Typography>
            <Typography variant="h4">ISBN: {listing.isbn10}</Typography>
            <Typography variant="h4">{listing.condition}</Typography>
            <Typography variant="h4">{listing.price}</Typography>
            {listing.contactOption === 0 && (
              <Button
                onClick={() => open()}
                color="primary"
                variant="contained"
              >
                Contact Seller
              </Button>
            )}
          </Grid>
          <Dialog open={isOpen} onClose={close}>
            <DialogContent>
              <ContactSellerForm
                listing={listing}
                onComplete={() => {
                  close();
                  alert(
                    "Your message was sent to the Seller, please wait for a reply in your inbox."
                  );
                }}
              />
            </DialogContent>
          </Dialog>
        </>
      )}
    </Grid>
  );
};

export default ListingDetails;
