import React from "react";
import { Link } from "@reach/router";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography
} from "@material-ui/core";
import styled from "styled-components";

type Props = {
  listing: {
    title: string,
    price: string,
    id: string,
    isbn10: String,
    isbn13: String,
    condition: string,
    authors: string
  }
};
const StyledCard = styled(Card)`
  height: 20em;
  width: 20em;
`;

const ListingCard = ({
  listing: { title, price, id, isbn10, isbn13, condition, authors }
}: Props) => (
  <Grid item>
    {/* //$FlowFixMe */}
    <Link to={`/listing/${id}`}>
      <StyledCard raised>
        <CardHeader
          title={
            <>
              <Typography variant="h5">{title}</Typography>
              <Typography variant="subtitle1">By: {authors}</Typography>
            </>
          }
        />
        <CardContent>
          <Typography variant="body1">{condition}</Typography>
          <Typography variant="body1">{isbn10}</Typography>
          <Typography variant="body1">{isbn13}</Typography>
          <Typography variant="body1">${price}</Typography>
        </CardContent>
      </StyledCard>
    </Link>
  </Grid>
);

export default ListingCard;
