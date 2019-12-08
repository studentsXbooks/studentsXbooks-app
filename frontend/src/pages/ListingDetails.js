import React from "react";
import {
  Grid,
  Typography,
  Button,
  Dialog,
  DialogContent
} from "@material-ui/core";
import styled from "styled-components";
import useApi from "../hooks/useApi";
import ContactSellerForm from "../components/ContactSellerForm";
import withSearchBar from "../components/withSearchBar";
import useToggle from "../hooks/useToggle";
import FallbackImage from "../components/ImageWithFallback";

const OptionBox = styled.div`
  border: 3px solid #ccc;
  border-radius: 5px;
  padding: 2rem;
`;

const ImageBox = styled.div`
  padding: 1rem;
  padding-top: 0;
  display: grid;
  grid-template-rows: 1fr;
  grid-template-cols: 1fr;
`;

type Props = {
  id: string
};

const ListingDetails = ({ id }: Props) => {
  const { data: listing } = useApi(`listings/${id}`);
  const [isOpen, open, close] = useToggle();

  return (
    <>
      {listing && (
        <Grid container spacing={3} justify="space-around">
          <Grid item>
            <ImageBox>
              <FallbackImage src={listing.thumbnail} alt="Book" />
              <Typography variant="h4" align="left">
                ISBN-10: {listing.isbn10}
              </Typography>
              <Typography variant="h4" align="left">
                ISBN-13: {listing.isbn13 || "None"}
              </Typography>
            </ImageBox>
          </Grid>
          <Grid item xs={8} container spacing={3}>
            <Grid item xs={12} sm={6} md={8}>
              <Typography variant="h2" gutterBottom>
                {listing.title}
              </Typography>
              <Typography variant="subtitle1" gutterBottom>
                By: {listing.authors}
              </Typography>
              <hr />
              <Typography variant="body1">{listing.description}</Typography>
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <OptionBox>
                <Grid container spacing={3} justify="space-between">
                  <Grid item>
                    <Typography variant="h4" style={{ gridArea: "condition" }}>
                      {listing.condition}
                    </Typography>
                  </Grid>
                  <Grid item>
                    <Typography
                      variant="h4"
                      style={{ gridArea: "price" }}
                      align="right"
                    >
                      $ {listing.price}
                    </Typography>
                  </Grid>
                </Grid>
                {listing.contactOption === 0 && (
                  <>
                    <Typography
                      variant="body2"
                      gutterBottom
                      style={{ gridArea: "message" }}
                    >
                      Message the seller now to make an offer or trade them a
                      book.
                    </Typography>
                    <Button
                      onClick={() => open()}
                      color="primary"
                      variant="contained"
                      fullWidth
                    >
                      Contact Seller
                    </Button>
                  </>
                )}
              </OptionBox>
            </Grid>
          </Grid>
        </Grid>
      )}
      {listing && (
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
      )}
    </>
  );
};

export default withSearchBar(ListingDetails);
