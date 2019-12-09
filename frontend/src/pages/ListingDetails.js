import React from "react";
import {
  Grid,
  Typography,
  Button,
  Dialog,
  DialogContent
} from "@material-ui/core";
// $FlowFixMe
import styled from "styled-components";
import useApi from "../hooks/useApi";
import ContactSellerForm from "../components/ContactSellerForm";
import withSearchBar from "../components/withSearchBar";
import useToggle from "../hooks/useToggle";

// $FlowFixMe
import { MuiThemeProvider, createMuiTheme } from "@material-ui/core";

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

const MaoButton = createMuiTheme({
  palette: {
    primary: {
      main: "#33578c"
    }
  }
});

const ListingDetails = ({ id }: Props) => {
  const { data: listing } = useApi(`listings/${id}`);
  const [isOpen, open, close] = useToggle();

  return (
    <>
      {listing && (
        <Grid container spacing={3} justify="space-around">
          <Grid item>
            <ImageBox>
              <img src="https://via.placeholder.com/500" alt="Book" />
              <Typography variant="h4" align="left">
                ISBN: {listing.isbn10}
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
                    <Typography
                      variant="h4"
                      spacing={5}
                      style={{ gridArea: "condition" }}
                    >
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
                      <br></br>
                    </Typography>
                    <MuiThemeProvider theme={MaoButton}>
                      <Button
                        onClick={() => open()}
                        color="primary"
                        variant="contained"
                        fullWidth
                      >
                        Make an offer
                      </Button>
                    </MuiThemeProvider>
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
